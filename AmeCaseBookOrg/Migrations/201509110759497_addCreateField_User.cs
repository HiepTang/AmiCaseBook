namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCreateField_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CreatedDate");
        }
    }
}
