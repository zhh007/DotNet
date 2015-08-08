using dotnet.NetExt;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Fang
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch _stopWatch = new Stopwatch();
            _stopWatch.Start();
            I9FanHandler.Run();
            _stopWatch.Stop();
            Console.WriteLine("执行时间:{0}", _stopWatch.Elapsed);

            //I9FanHandler.ParseDetailPage();
            //I9FanHandler.BuildHtmlFile();

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }

}
