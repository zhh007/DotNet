namespace Fang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PageUrls",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 500),
                        HasGet = c.Boolean(nullable: false),
                        IsPersonPost = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PageUrls");
        }
    }
}
