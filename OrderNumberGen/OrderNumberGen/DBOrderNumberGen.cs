using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Data;

namespace OrderNumberGen
{
    public class DBOrderNumberGen
    {
        private string _Gen(int custId)
        {
            int index = 1;
            DateTime t = DateTime.Now;
            string ID = "OrderNumberSeed";
            OrderNumberSeed seed = null;
            using (var context = new JFContext())
            {
                seed = context.OrderNumberSeeds.Where(p => p.ID == ID).FirstOrDefault();

                if (seed != null)
                {
                    if (seed.Time.Day == t.Day && seed.Time.Month == t.Month && seed.Time.Year == t.Year)
                    {
                        index = seed.Tick + 1;
                    }
                    seed.Tick = index;
                    
                    //context.OrderNumberSeeds.Attach(seed);
                    //context.Entry(seed).State = EntityState.Modified;//强制修改状态
                }
                else
                {
                    seed = new OrderNumberSeed
                    {
                        ID = ID,
                        Time = DateTime.Now,
                        Tick = index
                    };
                    context.OrderNumberSeeds.Add(seed);
                }

                context.SaveChanges();
            }

            return string.Format("{0:yyyyMMdd}{1:000000}{2:0000}", t, index, custId);
        }

        public string Gen(int custId)
        {
            try
            {
                return _Gen(custId);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                return Gen(custId);
            }
        }
    }

    public class DBOrderNumberGenThreadWrap
    {
        class LineClass
        {
            public string OrderNumber { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
        }

        public void Run(object threadIndex)
        {
            int custId = 88888;
            List<LineClass> list = new List<LineClass>();
            for (int i = 0; i < 1000; i++)
            {
                LineClass lc = new LineClass();
                DBOrderNumberGen gen = new DBOrderNumberGen();
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

    public class OrderNumberSeed
    {
        public string ID { get; set; }
        public DateTime Time { get; set; }
        public int Tick { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class OrderNumberSeedMap : EntityTypeConfiguration<OrderNumberSeed>
    {
        public OrderNumberSeedMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.RowVersion)
                .IsRequired()
                .IsRowVersion()
                .IsConcurrencyToken();

            this.Property(t => t.Time)
                .IsRequired();

            this.Property(t => t.Tick)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("OrderNumberSeed");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
            this.Property(t => t.Time).HasColumnName("Time");
            this.Property(t => t.Tick).HasColumnName("Tick");
        }
    }

    public class JFContext : DbContext
    {
        static JFContext()
        {
            Database.SetInitializer<JFContext>(null);
        }

        public JFContext()
            : base("Name=JFContext")
        {
        }

        public DbSet<OrderNumberSeed> OrderNumberSeeds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrderNumberSeedMap());
        }
    }

    public class TestDBOrderNumberGen
    {
        public static void Test()
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 10; i++)
            {
                DBOrderNumberGenThreadWrap threadWrap = new DBOrderNumberGenThreadWrap();
                Thread t = new Thread(new ParameterizedThreadStart(threadWrap.Run));
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
    }
}
