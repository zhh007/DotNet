namespace EF6Demo.Migrations
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
                        UnitPrice = c.Decimal(nullable: false, storeType: "money"),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Category", t => t.CategoryID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        ProfileID = c.Int(nullable: false),
                        Name = c.String(),
                        Sex = c.Boolean(),
                        Birthday = c.DateTime(),
                        Email = c.String(),
                        Telephone = c.String(),
                        Mobilephone = c.String(),
                        Address = c.String(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProfileID)
                .ForeignKey("dbo.Users", t => t.ProfileID)
                .Index(t => t.ProfileID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        IsValid = c.Boolean(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfiles", "ProfileID", "dbo.Users");
            DropForeignKey("dbo.Product", "CategoryID", "dbo.Category");
            DropIndex("dbo.UserProfiles", new[] { "ProfileID" });
            DropIndex("dbo.Product", new[] { "CategoryID" });
            DropTable("dbo.Users");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Product");
            DropTable("dbo.Category");
        }
    }
}
