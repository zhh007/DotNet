using EF6DemoFluent.Mapping;
using EF6DemoFluent.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6DemoFluent
{
    public partial class EF6DemoFluentContext : DbContext
    {
        public EF6DemoFluentContext()
            : base("EF6DemoFluent")
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //禁用一对多级联删除
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //禁用多对多级联删除
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ProductMap());

            //modelBuilder.Entity<Category>()
            //   .HasMany(t => t.Products)
            //   .WithRequired(t => t.Category)
            //   .HasForeignKey(d => d.CategoryID);

            //modelBuilder.Entity<Product>()
            //    .HasRequired(t => t.Category)
            //    .WithMany(t => t.Products)
            //    .HasForeignKey(d => d.CategoryID);
        }
    }
}
