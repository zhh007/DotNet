﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace OrderNumberGen
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in LockOrderNumberGen.randomPermutation(0, 9))
            {
                Console.Write(item);
                Console.Write(",");
            }
            Console.WriteLine("");

            foreach (var item in LockOrderNumberGen.randomPermutation(0, 9))
            {
                Console.Write(item);
                Console.Write(",");
            }
            Console.WriteLine("");


            //LockOrderNumberGenTest.Test();

            //DBOrderNumberGen gen = new DBOrderNumberGen();
            //Console.WriteLine(gen.Gen(12345));

            //Console.WriteLine(gen.Gen(12345));

            //TestDBOrderNumberGen.Test();

            //ShowLog();
            
            Console.ReadKey();
        }

        static void ShowLog()
        {
            //统计
            List<LogLine> list = new List<LogLine>();
            for (int i = 0; i < 10000; i++)
            {
                using (FileStream fs = new FileStream(string.Format("on_{0}.txt", i), FileMode.Open))
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] tmp = line.Split(new char[] { ',' });
                        LogLine ll = new LogLine();
                        ll.OrderNumber = tmp[0];
                        ll.Tick = Convert.ToInt64(tmp[1]);
                        ll.Span = new TimeSpan(ll.Tick);
                        list.Add(ll);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            int count = list.Count;
            int disCount = (from p in list
                            select p.OrderNumber).Distinct().Count();

            sb.AppendFormat("{0} - {1}\r\n", count, disCount);
            if (count != disCount)
            {
                sb.AppendFormat("重复{0}次\r\n", count - disCount);
            }

            //超过2秒的记录数
            var cst = (from p in list
                       where p.Span.TotalSeconds > 2
                       select p).Count();
            sb.AppendFormat("计算时间超过2秒的记录数：{0}\r\n", cst);

            //
            var lst = (from p in list
                       orderby p.Tick descending
                       select p).Take(10);

            sb.AppendFormat("耗时最长的10个记录。\r\n");
            foreach (var item in lst)
            {
                sb.AppendFormat("{0},{1}分{2}秒{3}毫秒\r\n", item.OrderNumber, item.Span.Minutes, item.Span.Seconds, item.Span.Milliseconds);
            }

            //Save to file
            using (FileStream fs = new FileStream("summary.txt", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(sb.ToString());
            }

            Console.Write(sb.ToString());

        }

    }

    public class LogLine
    {
        public string OrderNumber { get; set; }
        public long Tick { get; set; }
        public TimeSpan Span { get; set; }
    }
}
