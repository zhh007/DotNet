namespace EF6Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        Password = c.String(maxLength: 100),
                        IsValid = c.Boolean(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        RoleID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleID, t.UserID })
                .ForeignKey("dbo.Role", t => t.RoleID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.RoleID)
                .Index(t => t.UserID);

            var baseDir = AppDomain.CurrentDomain.BaseDirectory + "\\Sql";
            SqlFile(baseDir + "\\init.sql");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserID", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleID", "dbo.Role");
            DropIndex("dbo.UserRole", new[] { "UserID" });
            DropIndex("dbo.UserRole", new[] { "RoleID" });
            DropTable("dbo.UserRole");
            DropTable("dbo.User");
            DropTable("dbo.Role");
        }
    }
}
