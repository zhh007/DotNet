using dotnet.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start.");
            using (ConcurrentRunner concurrentTest = new ConcurrentRunner())
            {
                var result = concurrentTest.Execute(20, 0, 10, new TestClass().Execute);
                foreach (var item in result)
                {
                    Console.WriteLine("线程数：{0}\t成功：{1}\t失败：{2}\t耗时：{3}",
                        item.ThreadCount, item.SuccessCount, item.FailureCount, item.ElapsedMilliseconds);

                }
            }
            Console.WriteLine("over!");
            Console.ReadKey(true);
        }
    }

    public class TestClass
    {
        public bool Execute()
        {
            try
            {
                int num = GetRandom();
                LogHelper.Log("log test.", num.ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private int GetRandom()
        {
            return new Random().Next(0, 1000);
        }
    }
}
