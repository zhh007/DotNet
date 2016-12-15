namespace AutoMigration1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStudentNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "No", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "No");
        }
    }
}
