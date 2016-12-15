using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoMigration2
{
    public partial class EF6DemoContext : DbContext
    {
        static EF6DemoContext()
        {
            Database.SetInitializer<EF6DemoContext>(null);
        }

        public EF6DemoContext()
            : base("Name=EF6Demo")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = Assembly.GetAssembly(typeof(EF6DemoContext));
            modelBuilder.Configurations.AddFromAssembly(assembly);
        }

        public void InitializeDatabase()
        {
            if (!this.Database.Exists() || !this.Database.CompatibleWithModel(false))
            {
                this.Database.CreateIfNotExists();
                var configuration = new Migrations.Configuration();
                var migrator = new DbMigrator(configuration);

                //migrator.Configuration.TargetDatabase = new DbConnectionInfo(context.Database.Connection.ConnectionString, "System.Data.SqlClient");
                //var migrations = migrator.GetPendingMigrations();
                //if (migrations.Any())
                //{
                //    var scriptor = new MigratorScriptingDecorator(migrator);
                //    var script = scriptor.ScriptUpdate(null, migrations.Last());

                //    if (!string.IsNullOrEmpty(script))
                //    {
                //        context.Database.ExecuteSqlCommand(script);
                //    }
                //}

                migrator.Update();
            }
        }
    }
}
