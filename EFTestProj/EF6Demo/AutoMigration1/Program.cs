using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMigration1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (EF6DemoContext context = new EF6DemoContext())
            {
                context.Database.Initialize(false);
            }
        }
    }
}
