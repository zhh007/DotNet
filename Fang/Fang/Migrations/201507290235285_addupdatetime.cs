namespace Fang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addupdatetime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageUrls", "UpdateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageUrls", "UpdateTime");
        }
    }
}
