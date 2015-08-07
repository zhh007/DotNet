using CsQuery;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fang
{
    public class DetailPageProcesser
    {
        public static string[] blockedPoster = new string[] {
            "19楼VIP大亨", "叫我暴君", "时光房子找我","星愿租房","zhaohai198213","zhengzhiwu5520",
            "我爱我家房产租赁","1144307450明","陶逃淘","zyf168","明天会更好噢耶","zhaohai198213",
            "wangxuan819","向钱看齐12","zyf168"
        };

        public static void Run()
        {
            using (FangContext db = new FangContext())
            {
                var list = (from p in db.PageUrls
                            where p.HasGet == false
                            select p).ToList();

                //var list = (from p in db.PageUrls
                //            where p.ID == 2918
                //            select p).ToList();

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
                        CQ dom = File.ReadAllText(fpath, Encoding.GetEncoding("gbk"));
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=gb2312\" />");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<table>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td>日期</td><td>发布人</td><td>链接</td>");
            sb.AppendLine("</tr>");

            DateTime dt = new DateTime(2015, 7, 1);
            using (FangContext db = new FangContext())
            {
                var list = (from p in db.PageUrls
                            where p.IsPersonPost == true && p.UpdateTime > dt
                            orderby p.UpdateTime descending
                            select p).ToList();

                int i = 1;
                int len = list.Count;
                foreach (var item in list)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendFormat("<td>{0:yyyy-MM-dd}</td>", item.UpdateTime);
                    sb.AppendFormat("<td>{0}</td>", item.Author);
                    sb.AppendFormat("<td><a href='{0}' target='_blank'>{1}</a></td>", item.Url, item.Title);
                    sb.AppendLine("</tr>");
                    i++;
                }
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
    }
}
