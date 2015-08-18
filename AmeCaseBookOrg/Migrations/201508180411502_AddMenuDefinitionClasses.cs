namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMenuDefinitionClasses : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Category", name: "MainCategoryCode", newName: "ParentCategoryCode");
            RenameIndex(table: "dbo.Category", name: "IX_MainCategoryCode", newName: "IX_ParentCategoryCode");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Category", name: "IX_ParentCategoryCode", newName: "IX_MainCategoryCode");
            RenameColumn(table: "dbo.Category", name: "ParentCategoryCode", newName: "MainCategoryCode");
        }
    }
}
