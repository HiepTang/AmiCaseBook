namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_DataItem_Content_NText : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DataItem", "Content", c => c.String(nullable: false, storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataItem", "Content", c => c.String(nullable: false));
        }
    }
}
