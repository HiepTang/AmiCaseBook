namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataItemChangeMainCategoryToMainMenu : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DataItem", name: "MainCategoryID", newName: "MainMenuID");
            RenameIndex(table: "dbo.DataItem", name: "IX_MainCategoryID", newName: "IX_MainMenuID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.DataItem", name: "IX_MainMenuID", newName: "IX_MainCategoryID");
            RenameColumn(table: "dbo.DataItem", name: "MainMenuID", newName: "MainCategoryID");
        }
    }
}
