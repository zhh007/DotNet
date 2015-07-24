using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadWordTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //string badwords = System.IO.File.ReadAllText("data_filter.txt");
            //string[] bd = badwords.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            string[] bd = new string[] { "fuck" };
            //BadWordsFilter bad1 = new BadWordsFilter();
            //bad1.Init(bd);

            string content = "那个确定不错..主要是进行了大量优化fuck";

            //FastFilter ff = new FastFilter();
            //foreach (string w in bd)
            //{
            //    ff.AddKey(w);
            //}

            FilterKeyWords.Init(bd);
            string[] finded;

            //bd = new string[] { @"六四運\動" };
            //Badword.Init(bd);

            //if (bad1.HasBadWord(content))
            //if (ff.HasBadWord(content))
            if (FilterKeyWords.Find(content, out finded))
            //if (Badword.HasBlockWords(content))
            {
                Console.WriteLine("有敏感词。");
            }
            else
            {
                Console.WriteLine("正常。");
            }
            Console.ReadKey();
        }
    }
}
