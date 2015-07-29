namespace Fang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcreatetime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageUrls", "CreateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageUrls", "CreateTime");
        }
    }
}
