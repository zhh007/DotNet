using CsQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fang
{
    public class DetailPageProcesser
    {
        public static void Run()
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

                    if(File.Exists(fpath))
                    {
                        Console.WriteLine("{0}/{1} {2}", i, len, fpath);
                        CQ dom = File.ReadAllText(fpath, Encoding.GetEncoding("gbk"));
                        string title = dom["title"].Text();
                        string dt = dom[".cont-top-left meta"].Attr("content");
                        string content = dom[".view-data"].Text();

                        if (!string.IsNullOrEmpty(dt))
                        {
                            item.UpdateTime = Convert.ToDateTime(dt);
                        }
                        item.IsBlock = title.Contains("该帖被屏蔽");
                        if(!item.IsBlock)
                        { 
                            item.IsPersonPost = !content.Contains("同行");
                        }

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
    }
}
