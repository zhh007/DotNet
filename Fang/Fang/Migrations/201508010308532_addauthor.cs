namespace Fang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addauthor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageUrls", "Author", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageUrls", "Author");
        }
    }
}
