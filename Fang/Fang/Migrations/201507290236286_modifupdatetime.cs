namespace Fang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifupdatetime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PageUrls", "UpdateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PageUrls", "UpdateTime", c => c.DateTime(nullable: false));
        }
    }
}
