using AutoMigration1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMigration1
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EF6DemoContext, Migrations.Configuration>());

            using (EF6DemoContext context = new EF6DemoContext())
            {
                //可以不写
                //context.Database.Initialize(false);

                var student = context.Set<Student>().FirstOrDefault(p => p.Name == "abc");
                if(student != null)
                {
                    Console.WriteLine("{0}学号{1}.", student.Name, student.No);
                }
            }
            Console.ReadKey();
        }
    }
}
