namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Unlimited_DataItem_HtmlContentLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DataItem", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataItem", "Content", c => c.String(nullable: false, maxLength: 4000));
        }
    }
}
