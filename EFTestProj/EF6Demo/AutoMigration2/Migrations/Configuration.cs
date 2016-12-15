namespace AutoMigration2.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AutoMigration2.EF6DemoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AutoMigration2.EF6DemoContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Set<Student>().AddOrUpdate(
              p => p.Name,
              new Student { Name = "abc"
                //, No = "s0002"
              }
            );
        }
    }
}
