﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoMigration1
{
    public partial class EF6DemoContext : DbContext
    {
        //static EF6DemoContext()
        //{
        //    Database.SetInitializer<EF6DemoContext>(null);
        //}

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
