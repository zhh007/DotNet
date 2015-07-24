using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BadWordTest
{
    public class Badword
    {
        private static readonly Regex reg_b = new Regex(@"\B", RegexOptions.Compiled);
        private static readonly Regex reg_en = new Regex(@"[a-zA-Z]+", RegexOptions.Compiled);
        private static readonly Regex reg_num = new Regex(@"^[\-\.\s\d]+$", RegexOptions.Compiled);

        private static Regex reg_word = null; //组合所有屏蔽词的正则

        private static string[] words = null;

        private static Regex GetRegex()
        {
            if (reg_word == null)
            {
                string txt = GetPattern();
                reg_word = new Regex(txt, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            return reg_word;
        }

        public static void Init(string[] w)
        {
            for (int i = 0; i < w.Length; i++)
            {
                if (w[i].IndexOf("\\") > -1)
                {
                    w[i] = w[i].Replace("\\", "");
                    Console.WriteLine(w[i]);
                }
                if (w[i].IndexOf("(") > -1)
                {
                    w[i] = w[i].Replace("(", "");
                    Console.WriteLine(w[i]);
                }
                if (w[i].IndexOf(")") > -1)
                {
                    w[i] = w[i].Replace(")", "");
                    Console.WriteLine(w[i]);
                }
            }
            words = w;
        }

        /// <summary>
        /// 检查输入内容是否包含脏词（包含返回true）
        /// </summary>
        public static bool HasBlockWords(string raw)
        {
            return GetRegex().Match(raw).Success;
        }
        /// <summary>
        /// 脏词替换成*号
        /// </summary>
        public static string WordsFilter(string raw)
        {
            return GetRegex().Replace(raw, "***");
        }
        /// <summary>
        /// 获取内容中含有的脏词
        /// </summary>
        public static IEnumerable<string> GetBlockWords(string raw)
        {
            foreach (Match mat in reg_word.Matches(raw))
            {
                yield return (mat.Value);
            }
        }
        private static string GetPattern()
        {
            StringBuilder patt = new StringBuilder();
            string s;
            foreach (string word in GetBlockWords())
            {
                if (word.Length == 0) continue;
                if (word.Length == 1)
                {
                    patt.AppendFormat("|({0})", word);
                }
                else if (reg_num.IsMatch(word))
                {
                    patt.AppendFormat("|({0})", word);
                }
                else if (reg_en.IsMatch(word))
                {
                    s = reg_b.Replace(word, @"(?:[^a-zA-Z]{0,3})");
                    patt.AppendFormat("|({0})", s);
                }
                else
                {
                    s = reg_b.Replace(word, @"(?:[^\u4e00-\u9fa5]{0,3})");
                    patt.AppendFormat("|({0})", s);
                }
            }
            if (patt.Length > 0)
            {
                patt.Remove(0, 1);
            }
            return patt.ToString();
        }

        /// <summary>
        /// 获取所有脏词
        /// </summary>
        public static string[] GetBlockWords()
        {
            if (words != null)
            {
                return words;
            }
            return new string[] { };//这里应该从数据库获取
        }
    }
}
