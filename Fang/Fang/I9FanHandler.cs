using CsQuery;
using dotnet.NetExt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fang
{
    public class I9FanHandler
    {
        static int PageCount = 30;
        public static string[] blockedPoster = new string[] {
            "19楼VIP大亨", "叫我暴君", "时光房子找我","星愿租房","zhaohai198213","zhengzhiwu5520",
            "我爱我家房产租赁","1144307450明","陶逃淘","zyf168","明天会更好噢耶","zhaohai198213",
            "wangxuan819","向钱看齐12","zyf168","烨烨烨Mr","wlm5098590","我爱我家租房01"
        };

        public static void DownloadListPageAndParse()
        {
            string startUrl = "http://www." + string.Join("", new string[] { "1", "9", "lou" }) + ".com/thread/category/structure/search/result?m=10001&fid=1637&mf_1831_1=3&mf_1831_2=0&mf_55=2&mf_55_field=18&mf_68=0&mf_62=0";
            string requestEncoding = "gbk";
            string responseEncoding = "gbk";
            HttpUtil http = new HttpUtil(requestEncoding, responseEncoding);

            if (!Directory.Exists("ListPage"))
            {
                Directory.CreateDirectory("ListPage");
            }

            Random rnd = new Random();
            int index = 1;
            while (index <= PageCount)
            {
                Thread.Sleep(rnd.Next(0, 30) * 10);//0~300
                string url = startUrl;
                if (index > 1)
                {
                    url += "&page=" + index.ToString();
                }
                string txt = http.Get(url);
                string fpath = System.IO.Path.Combine(Environment.CurrentDirectory, "ListPage");
                fpath = System.IO.Path.Combine(fpath, index.ToString() + ".html");
                File.WriteAllText(fpath, txt, Encoding.UTF8);

                Console.WriteLine(index);

                index++;
            }

            int len = 1;
            while (len <= PageCount)
            {
                string fpath = System.IO.Path.Combine(Environment.CurrentDirectory, "ListPage");
                fpath = System.IO.Path.Combine(fpath, len.ToString() + ".html");

                CQ dom = File.ReadAllText(fpath);//, Encoding.GetEncoding("gbk")
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

        public static void DownloadDetailPage()
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
            WebBrowserUtil wbu = new WebBrowserUtil("gbk", "gbk");
            foreach (var item in urls)
            {
                string[] arr = item.ToString().Split(new char[] { '/' });
                filenameList.Add(arr[arr.Length - 1]);
            }

            wbu.DownloadPage(dir, urls, filenameList.ToArray());
        }

        public static void ParseDetailPage()
        {
            using (FangContext db = new FangContext())
            {
                var list = (from p in db.PageUrls
                            where p.HasGet == false
                            select p).ToList();

                int i = 1;
                int len = list.Count;
                foreach (var item in list)
                {
                    string[] arr = item.Url.ToString().Split(new char[] { '/' });
                    string filename = arr[arr.Length - 1];
                    string fpath = System.IO.Path.Combine(Environment.CurrentDirectory, "DetailPage");
                    fpath = System.IO.Path.Combine(fpath, filename);

                    if (File.Exists(fpath))
                    {
                        Console.WriteLine("{0}/{1} {2}", i, len, fpath);
                        CQ dom = File.ReadAllText(fpath);//, Encoding.GetEncoding("gbk")
                        string title = dom["title"].Text();
                        string dt = dom[".cont-top-left meta"].Attr("content");
                        string content = dom[".view-data"].Text();
                        string author = dom[".author a"].Attr("title");
                        bool isperson = true;

                        if (!string.IsNullOrEmpty(dt))
                        {
                            item.UpdateTime = Convert.ToDateTime(dt);
                        }
                        if (title.Contains("该帖被屏蔽"))
                        {
                            item.IsBlock = true;
                            isperson = false;
                        }

                        if (content.Contains("同行"))
                        {
                            isperson = false;
                        }

                        if (blockedPoster.Contains(author))
                        {//黑名单
                            isperson = false;
                        }

                        item.IsPersonPost = isperson;
                        item.Author = author;
                        item.HasGet = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("{0}/{1}", i, len);
                    }

                    i++;
                }
            }
        }

        public static void BuildHtmlFile()
        {
            List<PageUrl> list = null;

            DateTime dt = new DateTime(2015, 7, 1);
            using (FangContext db = new FangContext())
            {
                list = (from p in db.PageUrls
                            where p.IsPersonPost == true && p.UpdateTime > dt
                            orderby p.UpdateTime descending
                            select p).ToList();
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=gb2312\" />");
            sb.AppendFormat("<title>{0}</title>", list.Count);
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<table>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td>日期</td><td>发布人</td><td>链接</td>");
            sb.AppendLine("</tr>");

            int i = 1;
            int len = list.Count;
            foreach (var item in list)
            {
                sb.AppendLine("<tr>");
                sb.AppendFormat("<td>{0:yyyy-MM-dd HH:mm:ss}</td>", item.UpdateTime);
                sb.AppendFormat("<td>{0}</td>", item.Author);
                sb.AppendFormat("<td><a href='{0}' target='_blank'>{1}</a></td>", item.Url, item.Title);
                sb.AppendLine("</tr>");
                i++;
            }
            sb.AppendLine("</table>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            string dir = Path.Combine(Environment.CurrentDirectory, "Summary");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string logname = Path.Combine(dir, string.Format("{0:HHmmss_fffffff}.html", DateTime.Now));
            using (StreamWriter fs = new StreamWriter(logname, false, System.Text.Encoding.Default))
            {
                fs.Write(sb.ToString());
            }

            //Process.Start("iexplore.exe", logname);
            Process.Start("chrome", logname);
        }

        public static void Run()
        {
            DownloadListPageAndParse();
            DownloadDetailPage();
            ParseDetailPage();
            BuildHtmlFile();
        }


    }
}
