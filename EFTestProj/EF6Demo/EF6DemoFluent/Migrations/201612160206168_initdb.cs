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
                        ParentID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Category", t => t.ParentID)
                .Index(t => t.ParentID);
            
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
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.Family",
                c => new
                    {
                        FamilyID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Sex = c.Boolean(),
                        Birthday = c.DateTime(),
                    })
                .PrimaryKey(t => t.FamilyID);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        RoleID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleID, t.UserID })
                .ForeignKey("dbo.Role", t => t.RoleID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.RoleID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.FamilyMemberRelationship",
                c => new
                    {
                        ParentID = c.Int(nullable: false),
                        ChildID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentID, t.ChildID })
                .ForeignKey("dbo.Family", t => t.ParentID)
                .ForeignKey("dbo.Family", t => t.ChildID)
                .Index(t => t.ParentID)
                .Index(t => t.ChildID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FamilyMemberRelationship", "ChildID", "dbo.Family");
            DropForeignKey("dbo.FamilyMemberRelationship", "ParentID", "dbo.Family");
            DropForeignKey("dbo.UserProfile", "ProfileID", "dbo.User");
            DropForeignKey("dbo.UserRole", "UserID", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleID", "dbo.Role");
            DropForeignKey("dbo.Product", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.Category", "ParentID", "dbo.Category");
            DropIndex("dbo.FamilyMemberRelationship", new[] { "ChildID" });
            DropIndex("dbo.FamilyMemberRelationship", new[] { "ParentID" });
            DropIndex("dbo.UserRole", new[] { "UserID" });
            DropIndex("dbo.UserRole", new[] { "RoleID" });
            DropIndex("dbo.UserProfile", new[] { "ProfileID" });
            DropIndex("dbo.Product", new[] { "CategoryID" });
            DropIndex("dbo.Category", new[] { "ParentID" });
            DropTable("dbo.FamilyMemberRelationship");
            DropTable("dbo.UserRole");
            DropTable("dbo.Family");
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Product");
            DropTable("dbo.Category");
        }
    }
}
