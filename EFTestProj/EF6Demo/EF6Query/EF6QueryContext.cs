using EF6Query.Mapping;
using EF6Query.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6Query
{
    public class EF6QueryContext : DbContext
    {
        static EF6QueryContext()
        {
            Database.SetInitializer<EF6QueryContext>(null);
        }

        public EF6QueryContext()
            : base("EF6Query")
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserWithRole> UserWithRoles { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Ignore<UserWithRole>();

            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RoleMap());

            modelBuilder.Configurations.Add(new UserWithRoleMap());

            modelBuilder.Configurations.Add(new ProductMap());
        }
    }
}
