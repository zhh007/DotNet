using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace OrderNumberGen
{
    public class DBOrderNumberGen2
    {
        private string _Gen(int custId)
        {
            string code = "1";

            OrderNumberSeed2 seed = new OrderNumberSeed2();
            using (var context = new JFContext())
            {
                context.OrderNumberSeeds2.Add(seed);
                context.SaveChanges();
            }

            return string.Format("{0}{1:000000}{2:0000}", code, seed.ID, custId);
        }

        public string Gen(int custId)
        {
            try
            {
                return _Gen(custId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                return Gen(custId);
            }
        }
    }

    public class OrderNumberSeed2
    {
        public int ID { get; set; }
    }

    public class OrderNumberSeed2Map : EntityTypeConfiguration<OrderNumberSeed2>
    {
        public OrderNumberSeed2Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("OrderNumberSeed2");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }

    public class TestDBOrderNumberGen2
    {
        public static void Test()
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Run));
                threads.Add(t);
                t.Start(i);
                Console.WriteLine("Thread {0} start.", i);
            }

            // Wait for all the threads to finish so that the results list is populated.
            // If a thread is already finished when Join is called, Join will return immediately.
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //


        }

        class LineClass
        {
            public string OrderNumber { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
        }

        public static void Run(object threadIndex)
        {
            int custId = 99999;
            List<LineClass> list = new List<LineClass>();
            for (int i = 0; i < 1000; i++)
            {
                LineClass lc = new LineClass();
                DBOrderNumberGen2 gen = new DBOrderNumberGen2();
                lc.Begin = DateTime.Now;
                lc.OrderNumber = gen.Gen(custId);
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
