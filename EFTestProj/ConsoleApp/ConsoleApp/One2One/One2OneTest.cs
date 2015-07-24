using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.One2One
{
    public class A
    {
        public int AID { get; set; }
        public string Title { get; set; }
        public B B { get; set; }
    }

    public class B
    {
        public int AID { get; set; }
        public string Body { get; set; }
        public A A { get; set; }
    }

    public class AMap : EntityTypeConfiguration<A>
    {
        public AMap()
        {
            // Primary Key
            this.HasKey(t => t.AID);

            // Properties
            this.Property(t => t.AID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("A");
            this.Property(t => t.AID).HasColumnName("AID");
            this.Property(t => t.Title).HasColumnName("Title");

            // Relationships
            //this.HasRequired(t => t.B)
            //    .WithRequiredPrincipal();

            this.HasRequired(t => t.B)
                .WithRequiredPrincipal(p => p.A);
        }
    }

    public class BMap : EntityTypeConfiguration<B>
    {
        public BMap()
        {
            // Primary Key
            this.HasKey(t => t.AID);

            // Properties
            this.Property(t => t.AID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("B");
            this.Property(t => t.AID).HasColumnName("AID");
            this.Property(t => t.Body).HasColumnName("Body");

            // Relationships
            //this.HasRequired(t => t.A)
            //    .WithRequiredDependent();

            this.HasRequired(t => t.A)
                .WithRequiredDependent(p => p.B);

            //this.HasRequired(t => t.A)
            //    .WithRequiredDependent(p => p.B)
            //    .Map(p => p.MapKey("AID"));
        }
    }

    public class One2OneContext : DbContext
    {
        static One2OneContext()
        {
            Database.SetInitializer<One2OneContext>(null);
        }

        public One2OneContext()
            : base("Name=One2OneContext")
        {
        }

        public DbSet<A> As { get; set; }
        public DbSet<B> Bs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AMap());
            modelBuilder.Configurations.Add(new BMap());
        }
    }

    public class One2OneTest
    {
        public static void Run()
        {
            var a = new A();
            a.Title = "title test";
            a.B = new B();
            a.B.Body = "body test";
            using (One2OneContext ef = new One2OneContext())
            {
                ef.Set<A>().Add(a);
                ef.SaveChanges();
            }

            //using (One2OneContext ef = new One2OneContext())
            //{
            //    var a = ef.Set<A>()
            //        .Include(p => p.B)
            //        .Where(p => p.AID == 6)
            //        .FirstOrDefault();
            //    Console.WriteLine("Title:{0}", a.Title);
            //    Console.WriteLine("Body:{0}", (a.B != null) ? a.B.Body: "");

            //    if(a.B == null)
            //    {
            //        a.B = new B();
            //        a.B.Body = "update";
            //        ef.SaveChanges();
            //    }
            //}

            //using (One2OneContext ef = new One2OneContext())
            //{
            //    var b = ef.Set<B>()
            //        .Include(p => p.A)
            //        .Where(p => p.AID == 1)
            //        .FirstOrDefault();
            //    Console.WriteLine("Title:{0}", b.A.Title);
            //    Console.WriteLine("Body:{0}", b.Body);
            //}

            //var b = new B();
            //b.AID = 8;
            //b.Body = "update.";
            //using (One2OneContext ef = new One2OneContext())
            //{
            //    ef.Set<B>().Add(b);
            //    ef.SaveChanges();
            //}
        }
    }
}
