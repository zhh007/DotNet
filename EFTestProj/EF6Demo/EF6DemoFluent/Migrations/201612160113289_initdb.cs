namespace EF6DemoFluent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 20),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Category", t => t.CategoryID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        ProfileID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Sex = c.Boolean(),
                        Birthday = c.DateTime(),
                        Email = c.String(nullable: false, maxLength: 100),
                        Telephone = c.String(maxLength: 50),
                        Mobilephone = c.String(maxLength: 20),
                        Address = c.String(maxLength: 200),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProfileID)
                .ForeignKey("dbo.User", t => t.ProfileID)
                .Index(t => t.ProfileID);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfile", "ProfileID", "dbo.User");
            DropForeignKey("dbo.Product", "CategoryID", "dbo.Category");
            DropIndex("dbo.UserProfile", new[] { "ProfileID" });
            DropIndex("dbo.Product", new[] { "CategoryID" });
            DropTable("dbo.User");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Product");
            DropTable("dbo.Category");
        }
    }
}
