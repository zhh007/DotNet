using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace ClearTFSBinding
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); //
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox1.Text))
            {
                return;
            }

            txtlog.AppendText(string.Format("=====begin=====\r\n"));

            var tfsFiles = getFiles(textBox1.Text, "*.vssscc|*.vspscc", SearchOption.AllDirectories);
            var projFiles = getFiles(textBox1.Text, "*.csproj|*.vspscc", SearchOption.AllDirectories);
            var slnFiles = getFiles(textBox1.Text, "*.sln", SearchOption.AllDirectories);

            if (tfsFiles != null && tfsFiles.Length > 0)
            {
                foreach (string fpath in tfsFiles)
                {
                    RemoveReadonlyAtt(fpath);
                    if (File.Exists(fpath))
                    {
                        File.Delete(fpath);
                    }
                    txtlog.AppendText(string.Format("delete {0}\r\n", fpath));
                }
            }

            if (projFiles != null && projFiles.Length > 0)
            {
                foreach (string fpath in projFiles)
                {
                    RemoveReadonlyAtt(fpath);
                    ClearProj(fpath);
                    txtlog.AppendText(string.Format("clear {0}\r\n", fpath));
                }
            }

            if (slnFiles != null && slnFiles.Length > 0)
            {
                foreach (string fpath in slnFiles)
                {
                    RemoveReadonlyAtt(fpath);
                    ClearSLN(fpath);
                    txtlog.AppendText(string.Format("clear {0}\r\n", fpath));
                }
            }

            txtlog.AppendText(string.Format("=====end=====\r\n"));
        }

        public string[] getFiles(string SourceFolder, string Filter, System.IO.SearchOption searchOption)
        {
            // ArrayList will hold all file names
            ArrayList alFiles = new ArrayList();

            // Create an array of filter string
            string[] MultipleFilters = Filter.Split('|');

            // for each filter find mathing file names
            foreach (string FileFilter in MultipleFilters)
            {
                // add found file names to array list
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            }

            // returns string array of relevant file names
            return (string[])alFiles.ToArray(typeof(string));
        }

        private void RemoveReadonlyAtt(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }
            FileAttributes attributes = File.GetAttributes(path);

            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                // Make the file RW
                attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
                File.SetAttributes(path, attributes);
                //Console.WriteLine("The {0} file is no longer RO.", path);
            }
        }

        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }

        private void ClearSLN(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string tempPath = Path.GetTempFileName();
            using (var reader = new StreamReader(filePath))
            {
                using (var writer = new StreamWriter(File.OpenWrite(tempPath)))
                {
                    bool beginCheck = false;
                    bool endCheck = false;
                    while (!reader.EndOfStream)
                    {
                        string lineStr = reader.ReadLine();
                        if (beginCheck == false && endCheck == false && lineStr.Contains("GlobalSection(TeamFoundationVersionControl)"))
                        {
                            beginCheck = true;
                            continue;
                        }

                        if (beginCheck == true)
                        {
                            if (endCheck == false)
                            {
                                if (lineStr.Contains("EndGlobalSection"))
                                {
                                    endCheck = true;
                                }
                                continue;
                            }
                        }

                        writer.WriteLine(lineStr);
                    }
                }
            }
            if (File.Exists(tempPath))
            {
                File.Delete(filePath);
                File.Move(tempPath, filePath);
            }
        }

        private void ClearProj(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string tempPath = Path.GetTempFileName();
            using (var reader = new StreamReader(filePath))
            {
                using (var writer = new StreamWriter(File.OpenWrite(tempPath)))
                {
                    while (!reader.EndOfStream)
                    {
                        string lineStr = reader.ReadLine();
                        if (lineStr.Contains(">SAK<"))
                        {
                            continue;
                        }

                        writer.WriteLine(lineStr);
                    }
                }
            }
            if (File.Exists(tempPath))
            {
                File.Delete(filePath);
                File.Move(tempPath, filePath);
            }
        }
    }
}
