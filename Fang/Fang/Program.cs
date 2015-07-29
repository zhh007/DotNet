using CsQuery;
using System;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using Microsoft.Win32;
using System.Collections.Generic;
using dotnet.NetExt;

namespace Fang
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProcessList();

            //GetDetailPageList();

            DetailPageProcesser.Run();

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        static void GetDetailPageList()
        {
            string dir = System.IO.Path.Combine(Environment.CurrentDirectory, "DetailPage");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string[] urls = null;
            using (FangContext db = new FangContext())
            {
                urls = (from p in db.PageUrls
                        where p.HasGet == false
                        select p.Url).ToArray();
            }

            List<string> filenameList = new List<string>();
            foreach (var item in urls)
            {
                string[] arr = item.ToString().Split(new char[] { '/' });
                filenameList.Add(arr[arr.Length - 1]);
            }

            WebBrowserUtil wbu = new WebBrowserUtil();
            wbu.DownloadPage(dir, urls, filenameList.ToArray());
        }

        static void ProcessList()
        {
            string startUrl = "http://www." + string.Join("", new string[] { "1", "9", "lou" }) + ".com/thread/category/structure/search/result?m=10001&fid=1637&mf_1831_1=3&mf_1831_2=0&mf_55=2&mf_55_field=18&mf_68=0&mf_62=0";
            string requestEncoding = "gbk";
            string responseEncoding = "gbk";
            HttpUtil http = new HttpUtil(requestEncoding, responseEncoding);

            if (!Directory.Exists("ListPage"))
            {
                Directory.CreateDirectory("ListPage");
            }

            int index = 1;
            while (index <= 50)
            {
                Thread.Sleep(500);
                string url = startUrl;
                if (index > 1)
                {
                    url += "&page=" + index.ToString();
                }
                string txt = http.Get(url);
                string fpath = System.IO.Path.Combine(Environment.CurrentDirectory, "ListPage");
                fpath = System.IO.Path.Combine(fpath, index.ToString() + ".html");
                File.WriteAllText(fpath, txt, Encoding.GetEncoding("gbk"));

                Console.WriteLine(index);

                index++;
            }

            int len = 1;
            while (len <= 50)
            {
                string fpath = System.IO.Path.Combine(Environment.CurrentDirectory, "ListPage");
                fpath = System.IO.Path.Combine(fpath, len.ToString() + ".html");

                CQ dom = File.ReadAllText(fpath, Encoding.GetEncoding("gbk"));
                var list = dom[".list-data > tbody"].Find("div.subject > a");

                if (list != null && list.Length > 0)
                {
                    using (FangContext db = new FangContext())
                    {
                        foreach (var item in list)
                        {
                            PageUrl page = new PageUrl();
                            page.Url = item.Attributes["href"];
                            page.HasGet = false;
                            page.IsPersonPost = false;
                            page.Title = item.FirstChild.NodeValue.Trim();
                            page.CreateTime = DateTime.Now;
                            page.IsBlock = false;
                            page.UpdateTime = null;

                            if (db.PageUrls.Count(p => p.Url == page.Url) == 0)
                            {
                                db.PageUrls.Add(page);
                            }
                        }

                        db.SaveChanges();
                    }
                }
                Console.WriteLine(len + "," + list.Length);
                len++;
            }
        }
    }

}
