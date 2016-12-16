namespace EF6Query.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EF6Query.EF6QueryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EF6Query.EF6QueryContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            var role = new Role() {
                RoleName = "TEST"
            };

            var users = new List<User>();
            for (int i = 0; i < 100; i++)
            {
                users.Add(new User { UserName = "abc_" + i.ToString(), Roles = { role } });
            }

            context.Users.AddOrUpdate(
              p => p.UserName, users.ToArray()
            );
        }
    }
}
