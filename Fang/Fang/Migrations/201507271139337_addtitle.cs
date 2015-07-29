namespace Fang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageUrls", "Title", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageUrls", "Title");
        }
    }
}
