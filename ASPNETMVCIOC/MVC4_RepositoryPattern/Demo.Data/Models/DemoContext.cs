using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Data.Models.Mapping;

namespace Demo.Data.Models
{
    public partial class DemoContext : DbContext
    {
        static DemoContext()
        {
            Database.SetInitializer<DemoContext>(null);
        }

        public DemoContext()
            : base("Name=DemoContext")
        {
        }

        public DbSet<UserInfo> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserInfoMap());
            
        }
    }
}
