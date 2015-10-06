using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string txtInfo = "支持搜索文件夹名称，如：trace\r\n支持文件名模糊查找，如：*.log";
            lblInfo.Text = txtInfo;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("trace");
            sb.AppendLine("exceptionlog");
            sb.AppendLine("UploadFiles");
            txtSetting.Text = sb.ToString();


            //for (int i = 20; i >= 1; i--)
            //{
            //    ListViewItem li = new ListViewItem();
            //    li.SubItems[0].Text = "";
            //    li.SubItems.Add("aaa" + i.ToString());
            //    this.lvResult.Items.Add(li);
            //}
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

            StatusLabel.Text = "正在扫描...";
            btnSearch.Enabled = false;
            btnBrowser.Enabled = false;
            txtSetting.Enabled = false;
            txtDir.Enabled = false;
            try
            {
                System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(txtDir.Text);
                foreach (var searchPattern in query)
                {
                    if (searchPattern.IndexOf(".") > -1)
                    {//files
                        SearchFiles(root, searchPattern);
                    }
                    else
                    {//folder
                        SearchFiles(root, searchPattern);
                        SearchFolders(root, searchPattern);
                    }
                }
            }
            catch (Exception)
            {

            }

            StatusLabel.Text = "扫描完成";
            btnSearch.Enabled = true;
            btnBrowser.Enabled = true;
            txtSetting.Enabled = true;
            txtDir.Enabled = true;
        }

        private List<string> paths = new List<string>();

        private void SearchFiles(System.IO.DirectoryInfo dir, string searchPattern)
        {
            var files = dir.GetFiles(searchPattern, System.IO.SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (paths.Contains(file.FullName))
                {
                    continue;
                }
                paths.Add(file.FullName);
                ListViewItem li = new ListViewItem();
                li.Tag = file;
                li.SubItems[0].Text = "";
                li.SubItems.Add(file.FullName);
                this.lvResult.Items.Add(li);
            }
        }

        private void SearchFolders(System.IO.DirectoryInfo dir, string searchPattern)
        {
            var folders = dir.GetDirectories(searchPattern, System.IO.SearchOption.AllDirectories);
            foreach (var folder in folders)
            {
                if (paths.Contains(folder.FullName))
                {
                    continue;
                }
                paths.Add(folder.FullName);
                ListViewItem li = new ListViewItem();
                li.Tag = folder;
                li.SubItems[0].Text = "";
                li.SubItems.Add(folder.FullName);
                this.lvResult.Items.Add(li);
            }
        }

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
            catch (Exception)
            {

            }

            StatusLabel.Text = "删除完成";
        }
    }
}
