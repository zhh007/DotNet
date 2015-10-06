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
        public Form1()
        {
            InitializeComponent();
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
        }
    }
}
