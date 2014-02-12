using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace OrderNumberGen
{
    class LockOrderNumberGen
    {
        //public static int[] randomPermutation(int min, int max)
        //{
        //    Random r = new Random();
        //    SortedList<int, int> perm = new SortedList<int, int>();
        //    for (int i = min; i <= max; i++) perm.Add(r.Next(), i);
        //    return perm.Values.ToArray();
        //}

        static DateTime _LastTime;
        static int index = 0;
        static object _lock = new object();

        public static string GenOrderNumber(int custID)
        {
            lock (_lock)
            {
                DateTime now = DateTime.Now;
                if (_LastTime == null)
                {
                    index = 1;
                    _LastTime = now;
                }
                else
                {
                    index++;
                }
                
                return string.Format("{0:yyyyMMddHHmmss} - {1:00000} - {2:0000}"
                    , now
                    , index
                    , custID);
            }

            //int rnum = (new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0)).Next(99999));

            //return string.Format("{0:yyyyMMddHHmmssfff} - {1:00000} - {2:0000}"
            //    , t
            //    //, (new Random().Next(10000, 99999))
            //    //, (new Random((int)t.Ticks).Next(10000, 99999))
            //    , rnum
            //    //, (new Random((int)(t.Ticks & 0xffffffffL) | (int)(t.Ticks >> 32)).Next(99999))
            //    , custID);
        }
    }

    public class LockOrderNumberGenTest
    {
        public static void Test()
        {
            List<Thread> workerThreads = new List<Thread>();

            for (int i = 0; i < 10000; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(ThreadRun));
                workerThreads.Add(t);
                t.Start(i);
                Console.WriteLine("Thread {0} start.", i);
            }

            // Wait for all the threads to finish so that the results list is populated.
            // If a thread is already finished when Join is called, Join will return immediately.
            foreach (Thread thread in workerThreads)
            {
                thread.Join();
            }

            
        }


        class LineClass
        {
            public string OrderNumber { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
        }

        static void ThreadRun(object threadIndex)
        {
            List<LineClass> list = new List<LineClass>();
            for (int i = 0; i < 1; i++)
            {
                LineClass lc = new LineClass();
                lc.Begin = DateTime.Now;
                lc.OrderNumber = LockOrderNumberGen.GenOrderNumber(7777);
                lc.End = DateTime.Now;
                list.Add(lc);
            }
            
            //Save to file
            using (FileStream fs = new FileStream(string.Format("on_{0}.txt", threadIndex), FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in list)
                {
                    TimeSpan ts = item.End - item.Begin;
                    sw.WriteLine("{0},{1}", item.OrderNumber, ts.Ticks);
                }
            }
        }



    }
}
