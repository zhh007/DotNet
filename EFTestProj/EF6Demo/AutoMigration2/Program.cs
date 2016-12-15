using AutoMigration2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMigration2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (EF6DemoContext context = new EF6DemoContext())
            {
                context.InitializeDatabase();

                var student = context.Set<Student>().FirstOrDefault(p => p.Name == "abc");
                if (student != null)
                {
                    Console.WriteLine("{0}学号{1}.", student.Name, "");
                }
            }
            Console.ReadKey();
        }
    }
}
