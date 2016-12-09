using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Reflection;
using UnityDemo.Config;

namespace UnityDemo
{
    class Program
    {
        public static IUnityContainer UnityContainer = new UnityContainer();
        static void Main(string[] args)
        {
            UnityContainer.AddNewExtension<Interception>();
                            //.Configure<Interception>()
                            //.SetDefaultInterceptorFor(t, new VirtualMethodInterceptor());

            Assembly thisAss = Assembly.GetExecutingAssembly();
            foreach (Module mod in thisAss.GetLoadedModules(false))
            {
                foreach (Type t in mod.GetTypes())
                {
                    //if (t.IsSubclassOf(typeof(ConfigBase)))
                    if(typeof(ConfigBase) == t)
                    {
                        continue;
                    }

                    if(typeof(ConfigBase).IsAssignableFrom(t))
                    {
                        UnityContainer.RegisterType(t, t, new ContainerControlledLifetimeManager(), new Interceptor<VirtualMethodInterceptor>(), new InterceptionBehavior<ConfigManagerInterceptionBehavior>());
                    }
                }
            }

            //UnityContainer.AddNewExtension<Interception>()
            //    .Configure<Interception>()
            //    .SetDefaultInterceptorFor<SomeEntityClass>(new TransparentProxyInterceptor());

            SomeEntityClass obj = UnityContainer.Resolve<SomeEntityClass>();
            Console.WriteLine(obj.StringProperty);
            Console.WriteLine(obj.IntProperty);

            Console.ReadKey();
        }
    }
}
/*
 * Policy Injection Matching Rules
https://msdn.microsoft.com/en-us/library/dn507448(v=pandp.30).aspx
*/
