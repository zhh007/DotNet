namespace Fang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addisblock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageUrls", "IsBlock", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageUrls", "IsBlock");
        }
    }
}
