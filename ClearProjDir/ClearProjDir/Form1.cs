using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ClearProjDir
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

            _updateListViewByFile = new UpdateListViewByFileDelegate(UpdateListViewByFile);
            _updateListViewByFolder = new UpdateListViewByFolderDelegate(UpdateListViewByFolder);
            _showPathDelegate = new ShowPathDelegate(ShowPath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string txtInfo = "支持搜索文件夹名称，如：trace\r\n支持文件名模糊查找，如：*.log";
            lblInfo.Text = txtInfo;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("trace");
            sb.AppendLine("exceptionlog");
            sb.AppendLine("UploadFiles");
            sb.AppendLine("bin");
            sb.AppendLine("obj");
            txtSetting.Text = sb.ToString();
        }

        /// <summary>
        /// 全选
        /// </summary>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResult.Items)
            {
                item.Checked = true;
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        private void btnSelectOther_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResult.Items)
            {
                item.Checked = !item.Checked;
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

        /// <summary>
        /// 扫描
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(txtDir.Text))
            {
                return;
            }

            string[] query = txtSetting.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (query == null || query.Length == 0)
            {
                return;
            }

            List<Regex> regList = new List<Regex>();
            foreach (var item in query)
            {
                string regStr = string.Format(@"(\\)*" + item.Replace("*", @"[^\\]*") + @"(\\)*");
                regList.Add(new Regex(regStr));
            }

            paths.Clear();
            //this.lvResult.Clear();
            //this.lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            //this.columnHeader2,
            //this.columnHeader1});

            this.BeginInvoke(new Action(delegate ()
            {
                while (lvResult.Items.Count > 0)
                    lvResult.Items.RemoveAt(0);
            }));

            StatusLabel.Text = "正在扫描...";
            btnSearch.Enabled = false;
            btnBrowser.Enabled = false;
            txtSetting.Enabled = false;
            txtDir.Enabled = false;
            btnSelect.Enabled = false;
            btnSelectOther.Enabled = false;
            btnDelete.Enabled = false;
            try
            {
                Thread t = new Thread(() =>
                {
                    ThreadMethod(txtDir.Text, regList);
                });
                t.Start();
            }
            catch (Exception)
            {
                StatusLabel.Text = "扫描完成";
                btnSearch.Enabled = true;
                btnBrowser.Enabled = true;
                txtSetting.Enabled = true;
                txtDir.Enabled = true;
                btnSelect.Enabled = true;
                btnSelectOther.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private List<string> paths = new List<string>();

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvResult.CheckedItems.Count == 0)
            {
                return;
            }

            StatusLabel.Text = "正在删除...";

            try
            {
                for (int i = lvResult.CheckedItems.Count - 1; i >= 0; i--)
                {
                    var item = lvResult.CheckedItems[i];
                    if (item.Tag is System.IO.DirectoryInfo)
                    {
                        var dir = item.Tag as System.IO.DirectoryInfo;
                        dir.Delete(true);
                    }
                    else if (item.Tag is System.IO.FileInfo)
                    {
                        var file = item.Tag as System.IO.FileInfo;
                        file.Delete();
                    }
                    lvResult.Items.RemoveAt(lvResult.CheckedIndices[i]);
                }
            }
            catch (Exception ex)
            {

            }

            StatusLabel.Text = "删除完成";
        }

        private void SearchFiles(System.IO.FileInfo[] files, List<Regex> regList)
        {
            foreach (var file in files)
            {
                //方法1
                //this.BeginInvoke(new Action(delegate () {
                //    StatusLabel.Text = file.FullName;
                //}));
                //方法2
                ShowPath(file.FullName);
                Thread.Sleep(1);

                bool fileIsMatch = false;
                foreach (var reg in regList)
                {
                    if (reg.IsMatch(file.FullName))
                    {
                        fileIsMatch = true;
                        break;
                    }
                }
                if (fileIsMatch)
                {
                    if (!paths.Contains(file.FullName))
                    {
                        paths.Add(file.FullName);
                        this.BeginInvoke(_updateListViewByFile, file);
                    }
                }
            }
        }

        private void SearchFolder(System.IO.DirectoryInfo dir, List<Regex> regList)
        {
            var files = dir.GetFiles();
            SearchFiles(files, regList);
            var dirs = dir.GetDirectories();
            foreach (var folder in dirs)
            {
                //方法1
                //this.BeginInvoke(new Action(delegate () {
                //    StatusLabel.Text = folder.FullName;
                //}));
                //方法2
                ShowPath(folder.FullName);
                Thread.Sleep(1);

                bool folderIsMatch = false;
                foreach (var reg in regList)
                {
                    if (reg.IsMatch(folder.FullName))
                    {
                        folderIsMatch = true;
                        break;
                    }
                }
                if (folderIsMatch)
                {
                    if (!paths.Contains(folder.FullName))
                    {
                        paths.Add(folder.FullName);
                        this.BeginInvoke(_updateListViewByFolder, folder);
                    }
                    continue;
                }
                SearchFolder(folder, regList);
            }
        }

        private void ThreadMethod(string dir, List<Regex> regList)
        {
            System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(dir);

            SearchFolder(root, regList);

            this.BeginInvoke(new Action(() =>
            {
                StatusLabel.Text = "扫描完成";
                btnSearch.Enabled = true;
                btnBrowser.Enabled = true;
                txtSetting.Enabled = true;
                txtDir.Enabled = true;
                btnSelect.Enabled = true;
                btnSelectOther.Enabled = true;
                btnDelete.Enabled = true;
            }));
        }

        public delegate void UpdateListViewByFileDelegate(System.IO.FileInfo file);
        public delegate void UpdateListViewByFolderDelegate(System.IO.DirectoryInfo folder);
        public delegate void ShowPathDelegate(string path);
        private UpdateListViewByFileDelegate _updateListViewByFile = null;
        private UpdateListViewByFolderDelegate _updateListViewByFolder = null;
        private ShowPathDelegate _showPathDelegate = null;
        private void UpdateListViewByFile(System.IO.FileInfo file)
        {
            ListViewItem li = new ListViewItem();
            li.Tag = file;
            li.SubItems[0].Text = "";
            li.SubItems.Add(file.FullName);
            this.lvResult.Items.Add(li);

            Trace.WriteLine(file.FullName);
        }
        private void UpdateListViewByFolder(System.IO.DirectoryInfo folder)
        {
            ListViewItem li = new ListViewItem();
            li.Tag = folder;
            li.SubItems[0].Text = "";
            li.SubItems.Add(folder.FullName);
            this.lvResult.Items.Add(li);

            Trace.WriteLine(folder.FullName);
        }
        private void ShowPath(string path)
        {
            if (this.InvokeRequired)
            {
                //方法1
                this.BeginInvoke(new MethodInvoker(delegate { ShowPath(path); }));
                //方法2
                //this.BeginInvoke(new MethodInvoker(() => ShowPath(path)));
            }
            else
            {
                this.StatusLabel.Text = path;
            }
        }
    }
}
