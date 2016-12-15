using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EF6Demo
{
    public partial class EF6DemoContext : DbContext
    {
        public EF6DemoContext()
            : base("EF6Demo")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = Assembly.GetAssembly(typeof(EF6DemoContext));
            modelBuilder.Configurations.AddFromAssembly(assembly);
        }
    }
}
