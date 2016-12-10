using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //本周开始时间和结束时间
            var dt = DateTime.Now;
            DateTime weekbegin = new DateTime(dt.Year, dt.Month, dt.Day).AddDays(-(int)dt.DayOfWeek + 1);
            DateTime weekend = weekbegin.AddDays(6).Add(new TimeSpan(23, 59, 59));

            Console.WriteLine(dt);
            Console.WriteLine(weekbegin);
            Console.WriteLine(weekend);

            //上周执行时间
            var lastbegin = new DateTime(2016, 11, 6, 9, 30, 0);
            var lastend = new DateTime(2016, 11, 6, 12, 30, 0);

            //计算本周执行时间
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            int daynum = 0;
            if(lastbegin.DayOfWeek == DayOfWeek.Sunday)
            {
                daynum = 6;
            }
            else
            {
                daynum = (int)lastbegin.DayOfWeek - 1;
            }
            var day = weekbegin.AddDays(daynum);

            //本周执行时间
            var begin = day.Add(new TimeSpan(lastbegin.Hour, lastbegin.Minute, lastbegin.Second));
            var end = day.Add(new TimeSpan(lastend.Hour, lastend.Minute, lastend.Second));

            Console.WriteLine("====================");
            Console.WriteLine(now);
            Console.WriteLine(begin);
            Console.WriteLine(end);

            Console.Read();
        }
    }
}
