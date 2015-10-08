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
            sb.AppendLine("UploadFiles");
            txtExcludeDir.Text = sb.ToString();

            sb = new StringBuilder();
            sb.AppendLine("*.log");
            txtExcludeFile.Text = sb.ToString();

            if(string.IsNullOrEmpty(ZipUtility.WinRARPath))
            {
                btnZip.Enabled = false;
                StatusLabel.Text = "未安装winrar";
            }
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

            try
            {
                this.Hide();
                ZipUtility.RARXFM(txtDir.Text, txtDir.Text, string.Format("{0}{1:yyyyMMddHHmmssffffff}.rar", root.Name, DateTime.Now), exp);
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Show();
            }
            StatusLabel.Text = "压缩结束";
            txtDir.Enabled = false;
            btnBrowser.Enabled = false;
            txtExcludeDir.Enabled = false;
            txtExcludeFile.Enabled = false;
            btnZip.Enabled = false;
        }

    }
}
