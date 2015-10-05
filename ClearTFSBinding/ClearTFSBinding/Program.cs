using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClearTFSBinding
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string curFolder = "";
            if (args != null && args.Length > 0)
            {
                if(System.IO.Directory.Exists(args[0]))
                {
                    curFolder = args[0];
                    Console.WriteLine(curFolder);
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(curFolder));
        }
    }
}
