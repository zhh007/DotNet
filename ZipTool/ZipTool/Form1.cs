using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZipTool
{
    public partial class Form1 : Form
    {
        string WinRARPath = string.Empty;
        public Form1(string dir)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(dir))
            {
                txtDir.Text = dir;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("bin");
            sb.AppendLine("obj");
            sb.AppendLine("trace");
            sb.AppendLine("exceptionlog");
            txtExcludeDir.Text = sb.ToString();

            sb = new StringBuilder();
            sb.AppendLine("*.log");
            txtExcludeFile.Text = sb.ToString();

            WinRARPath = "c:\\Program Files\\WinRAR\\Rar.exe";

            //var regkey = Registry.ClassesRoot.OpenSubKey(@"Applications\WinRAR.exe\shell\open\command");
            //var regvalue = regkey.GetValue("");  // 键值为 "d:\Program Files\WinRAR\WinRAR.exe" "%1"
            //WinRARPath = regvalue.ToString();
            //regkey.Close();
            //WinRARPath = WinRARPath.Substring(1, WinRARPath.Length - 7);  // d:\Program Files\WinRAR\WinRAR.exe
        }

        /// <summary>
        /// 浏览
        /// </summary>
        private void btnBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = false;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtDir.Text = dlg.SelectedPath;
            }
        }

        private void btnZip_Click(object sender, EventArgs e)
        {
            if(!System.IO.Directory.Exists(txtDir.Text))
            {
                return;
            }
            System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(txtDir.Text);

            StatusLabel.Text = "正在压缩...";
            txtDir.Enabled = false;
            btnBrowser.Enabled = false;
            txtExcludeDir.Enabled = false;
            txtExcludeFile.Enabled = false;
            btnZip.Enabled = false;

            List<string> exp = new List<string>();
            foreach (var item in txtExcludeDir.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    exp.Add(item);
                }
            }
            foreach (var item in txtExcludeFile.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    exp.Add(item);
                }
            }

            RARXFM(txtDir.Text, txtDir.Text, string.Format("{0}{1:yyyyMMddHHmmssffffff}.rar", root.Name, DateTime.Now), exp);

            StatusLabel.Text = "压缩结束";
            txtDir.Enabled = false;
            btnBrowser.Enabled = false;
            txtExcludeDir.Enabled = false;
            txtExcludeFile.Enabled = false;
            btnZip.Enabled = false;
        }

        /// <summary> 
        /// 利用 WinRAR 进行压缩 ,具体查看 http://www.okbase.net/doc/details/2582
        /// </summary> 
        /// <param name="path">将要被压缩的文件夹（绝对路径）</param> 
        /// <param name="rarPath">压缩后的 .rar 的存放目录（绝对路径）</param> 
        /// <param name="rarName">压缩文件的名称（包括后缀）</param> 
        /// <param name="xfiles">排除文件夹里所有文件，以及文件本身；多个文件直接以；隔开 如：bin ；obj</param> 
        /// <returns>true 或 false。压缩成功返回 true，反之，false。</returns> 
        public bool RARXFM(string path, string rarPath, string rarName, List<string> xfiles)
        {
            bool flag = false;
            //string rarexe;       //WinRAR.exe 的完整路径 
            //RegistryKey regkey;  //注册表键 
            //Object regvalue;     //键值 
            string cmd;          //WinRAR 命令参数 
            ProcessStartInfo startinfo;
            Process process;
            try
            {
                //rarexe = AppDomain.CurrentDomain.BaseDirectory + @"\WinRAR.exe";

                string targetpath = System.IO.Path.GetDirectoryName(path);

                if (!System.IO.Directory.Exists(targetpath))
                    System.IO.Directory.CreateDirectory(targetpath);

                string targetrarPath = System.IO.Path.GetDirectoryName(rarPath);//存放路径不存在 创建
                if (!System.IO.Directory.Exists(targetrarPath))
                    System.IO.Directory.CreateDirectory(targetrarPath);
                //压缩命令，相当于在要压缩的文件夹(path)上点右键->WinRAR->添加到压缩文件->输入压缩文件名(rarName) 
                cmd = string.Format("a {0} {1} -ep1 -o+ -inul -r -ibck",
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
                        arrtr += string.Format(" -x.\\{0}\\* -x.\\{0} ", str);
                    }
                    cmd = cmd + arrtr;
                }
                #endregion
                startinfo = new ProcessStartInfo();
                startinfo.FileName = WinRARPath;
                startinfo.Arguments = cmd;                          //设置命令参数 
                startinfo.WindowStyle = ProcessWindowStyle.Hidden;  //隐藏 WinRAR 窗口

                startinfo.WorkingDirectory = rarPath;
                process = new Process();
                process.StartInfo = startinfo;
                process.Start();
                process.WaitForExit(); //无限期等待进程 winrar.exe 退出 
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
    }
}
