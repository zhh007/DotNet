using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Pluggable;
using System.Collections.ObjectModel;

namespace PluginManage
{
    public class PluginManager
    {
        private static volatile PluginManager instance;
        private static object syncRoot = new Object();
        private static string ExecuteDir = "";
        private static string PluginDir = "";

        private static Dictionary<Type, PluggableDescriptor> Pluggables = new Dictionary<Type, PluggableDescriptor>();
        private static Dictionary<Type, Type> SingleLifeDict = new Dictionary<Type, Type>();
        private static Dictionary<Type, List<Type>> MultiLifeDict = new Dictionary<Type, List<Type>>();

        private PluginManager()
        {
            string cb = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            ExecuteDir = Path.GetDirectoryName(cb).Remove(0, 6);
            PluginDir = Path.Combine(ExecuteDir, "Plugins");

            LoadPluggables();

            LoadPlugins();

            WatchPluginDir();
        }

        public ReadOnlyCollection<PluggableDescriptor> GetAllPluginDescriptors()
        {
            List<PluggableDescriptor> list = new List<PluggableDescriptor>();

            foreach (var item in Pluggables.Keys)
            {
                list.Add(Pluggables[item]);
            }

            return new ReadOnlyCollection<PluggableDescriptor>(list);
        }

        public T GetPlugin<T>()
        {
            Type t = typeof(T);
            if (SingleLifeDict.ContainsKey(t))
            {
                Type impl = SingleLifeDict[t];
                return (T)Activator.CreateInstance(impl);
            }
            return default(T);
        }

        public List<T> GetPlugins<T>()
        {
            List<T> list = new List<T>();

            Type t = typeof(T);
            if (MultiLifeDict.ContainsKey(t))
            {
                foreach (Type impl in MultiLifeDict[t])
                {
                    list.Add((T)Activator.CreateInstance(impl));
                }
            }

            return list;
        }

        public static PluginManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PluginManager();
                    }
                }

                return instance;
            }
        }

        private void LoadPluggables()
        {
            IEnumerable<string> availableAssemblies = Directory.EnumerateFiles(ExecuteDir, "*.dll", SearchOption.TopDirectoryOnly);

            foreach (string assemblyPath in availableAssemblies)
            {
                try
                {
                    Assembly ass = Assembly.LoadFrom(assemblyPath);

                    Type[] types = ass.GetTypes();
                    int len = types.Length;
                    for (int i = 0; i < len; i++)
                    {
                        Type t = types[i];
                        var atts = t.GetCustomAttributes(typeof(PluggableAttribute), false);
                        if (atts != null && atts.Length > 0)
                        {
                            PluggableAttribute plugAtt = atts[0] as PluggableAttribute;
                            if (!Pluggables.Keys.Contains(t))
                            {
                                PluggableDescriptor desc = new PluggableDescriptor();
                                desc.Type = t;
                                desc.Name = plugAtt.Name;
                                desc.Mode = plugAtt.Mode;
                                desc.Description = plugAtt.Description;

                                Pluggables.Add(t, desc);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    if (ex is System.Reflection.ReflectionTypeLoadException)
                    {
                        System.Reflection.ReflectionTypeLoadException rex = ex as System.Reflection.ReflectionTypeLoadException;
                        if (rex.LoaderExceptions.Length > 0)
                        {
                            foreach (var item in rex.LoaderExceptions)
                            {

                                sb.AppendLine(item.Message);
                            }
                        }
                    }
                    Debug.WriteLine(sb.ToString());
                    //throw ex;
                }
            }
        }

        private void LoadPlugins()
        {
            if (!Directory.Exists(PluginDir))
            {
                return;
            }

            DirectoryInfo di = new DirectoryInfo(PluginDir);
            DirectoryInfo[] dirs = di.GetDirectories("*", SearchOption.TopDirectoryOnly);

            foreach (DirectoryInfo dir in dirs)
            {
                DirectoryInfo lastDir = dir.GetDirectories().OrderByDescending(x => x.Name).FirstOrDefault();
                string lastVersionDir = lastDir.FullName;
                foreach (string filepath in Directory.GetFiles(lastVersionDir, "*.dll"))
                {
                    LoadPluginFrom(filepath);
                }
            }
        }

        private void LoadPluginFrom(string filepath)
        {
            Assembly asm = Assembly.LoadFile(filepath);

            Type[] types = asm.GetTypes();
            int len = types.Length;
            for (int i = 0; i < len; i++)
            {
                Type impl = types[i];
                var atts = impl.GetCustomAttributes(typeof(PluginAttribute), false);
                if (atts != null && atts.Length > 0)
                {
                    var allInterfaces = impl.GetInterfaces();
                    foreach (var it in allInterfaces)
                    {
                        if (!Pluggables.ContainsKey(it))
                            continue;
                        PluggableDescriptor desc = Pluggables[it];
                        if (desc.Mode == PluginImplementMode.Single)
                        {
                            if (SingleLifeDict.ContainsKey(it))
                            {
                                SingleLifeDict[it] = impl;
                            }
                            else
                            {
                                SingleLifeDict.Add(it, impl);
                            }
                        }
                        else
                        {
                            if (MultiLifeDict.ContainsKey(it))
                            {
                                string tfullname = impl.FullName;
                                var list = MultiLifeDict[it];
                                for (int j = list.Count - 1; j >= 0; j--)
                                {
                                    if (list[j].FullName == tfullname)
                                    {
                                        list.RemoveAt(j);
                                        break;
                                    }
                                }
                                MultiLifeDict[it].Add(impl);
                            }
                            else
                            {
                                List<Type> list = new List<Type>();
                                list.Add(impl);
                                MultiLifeDict[it] = list;
                            }
                        }
                    }
                }
            }
        }

        private void WatchPluginDir()
        {
            FileSystemWatcher fsw = new FileSystemWatcher();
            fsw.Path = PluginDir;
            fsw.Filter = "*.dll";
            fsw.NotifyFilter = NotifyFilters.LastAccess |
                         NotifyFilters.LastWrite |
                         NotifyFilters.FileName |
                         NotifyFilters.DirectoryName;
            fsw.IncludeSubdirectories = true;
            fsw.Created += new FileSystemEventHandler(fsw_Created);
            fsw.EnableRaisingEvents = true;
        }

        private void fsw_Created(object sender, FileSystemEventArgs e)
        {
            LoadPluginFrom(e.FullPath);
        }
    }
}
