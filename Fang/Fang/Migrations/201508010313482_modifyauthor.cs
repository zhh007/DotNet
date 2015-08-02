namespace Fang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyauthor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PageUrls", "Author", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PageUrls", "Author", c => c.String());
        }
    }
}
