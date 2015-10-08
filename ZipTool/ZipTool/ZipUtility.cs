using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ZipTool
{
    public class ZipUtility
    {
        public static string WinRARPath = GetPath(".rar");

        /// <summary> 
        /// 利用 WinRAR 进行压缩 ,具体查看 http://www.okbase.net/doc/details/2582
        /// </summary> 
        /// <param name="path">将要被压缩的文件夹（绝对路径）</param> 
        /// <param name="rarPath">压缩后的 .rar 的存放目录（绝对路径）</param> 
        /// <param name="rarName">压缩文件的名称（包括后缀）</param> 
        /// <param name="xfiles">排除文件夹里所有文件，以及文件本身；多个文件直接以；隔开 如：bin ；obj</param> 
        /// <returns>true 或 false。压缩成功返回 true，反之，false。</returns> 
        public static bool RARXFM(string path, string rarPath, string rarName, List<string> xfiles)
        {
            bool flag = false;
            string cmd;//WinRAR 命令参数 
            ProcessStartInfo startinfo;
            Process process;
            try
            {
                string targetpath = System.IO.Path.GetDirectoryName(path);

                if (!System.IO.Directory.Exists(targetpath))
                    System.IO.Directory.CreateDirectory(targetpath);

                string targetrarPath = System.IO.Path.GetDirectoryName(rarPath);//存放路径不存在 创建
                if (!System.IO.Directory.Exists(targetrarPath))
                    System.IO.Directory.CreateDirectory(targetrarPath);
                //压缩命令，相当于在要压缩的文件夹(path)上点右键->WinRAR->添加到压缩文件->输入压缩文件名(rarName) 
                cmd = string.Format("a {0} {1} -ep1 -o+ -r -ibck",//-inul
                                    rarName,
                                    path);
                //-x*\\bin\\* -x*\\bin -x*\\obj\\* -x*\\obj     ---排除bin,obj文件夹里所有文件，以及文件本身
                #region 排除文件里所有文件已经文件本身
                if (xfiles != null && xfiles.Count > 0)
                {
                    string arrtr = string.Empty;
                    foreach (string str in xfiles)
                    {
                        //arrtr += string.Format(" -x*\\{0}\\* -x*\\{0} ", str);
                        arrtr += string.Format(" -x.\\*\\{0}\\* -x.\\*\\{0} ", str);
                    }
                    cmd = cmd + arrtr;
                }
                #endregion
                startinfo = new ProcessStartInfo();
                startinfo.FileName = WinRARPath;
                startinfo.Arguments = cmd;//设置命令参数 
                //startinfo.WindowStyle = ProcessWindowStyle.Hidden;//隐藏 WinRAR 窗口

                startinfo.WorkingDirectory = rarPath;
                process = new Process();
                process.StartInfo = startinfo;
                process.Start();
                process.WaitForExit();//无限期等待进程 winrar.exe 退出 
                if (process.HasExited)
                {
                    flag = true;
                }
                process.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return flag;
        }

        private static string GetPath(string extension)
        {
            string appPath = string.Empty;
            var appName = (string)Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(extension).GetValue(null);
            var openWith = (string)Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(appName + @"\shell\open\command").GetValue(null);
            if (openWith[0] == '"')
            {
                int pos2 = openWith.IndexOf('"', 1);
                appPath = openWith.Substring(1, pos2 - 1);
            }
            return appPath;
        }
    }
}
