namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHitToCommunityTopic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommunityTopic", "Hit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommunityTopic", "Hit");
        }
    }
}
