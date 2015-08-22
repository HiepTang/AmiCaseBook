namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataItemRelationWithMenu : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DataItem", "SubCategory_Code", "dbo.Category");
            DropForeignKey("dbo.DataItem", "MainCategory_Code", "dbo.Category");
            DropIndex("dbo.DataItem", new[] { "MainCategory_Code" });
            DropIndex("dbo.DataItem", new[] { "SubCategory_Code" });
            DropColumn("dbo.DataItem", "SubCategoryID");
            DropColumn("dbo.DataItem", "MainCategoryID");
            RenameColumn(table: "dbo.DataItem", name: "SubCategory_Code", newName: "SubCategoryID");
            RenameColumn(table: "dbo.DataItem", name: "MainCategory_Code", newName: "MainCategoryID");
            AlterColumn("dbo.DataItem", "MainCategoryID", c => c.Int(nullable: false));
            AlterColumn("dbo.DataItem", "SubCategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.DataItem", "MainCategoryID");
            CreateIndex("dbo.DataItem", "SubCategoryID");
            AddForeignKey("dbo.DataItem", "SubCategoryID", "dbo.Category", "Code", cascadeDelete: false);
            AddForeignKey("dbo.DataItem", "MainCategoryID", "dbo.Category", "Code", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DataItem", "MainCategoryID", "dbo.Category");
            DropForeignKey("dbo.DataItem", "SubCategoryID", "dbo.Category");
            DropIndex("dbo.DataItem", new[] { "SubCategoryID" });
            DropIndex("dbo.DataItem", new[] { "MainCategoryID" });
            AlterColumn("dbo.DataItem", "SubCategoryID", c => c.Int());
            AlterColumn("dbo.DataItem", "MainCategoryID", c => c.Int());
            RenameColumn(table: "dbo.DataItem", name: "MainCategoryID", newName: "MainCategory_Code");
            RenameColumn(table: "dbo.DataItem", name: "SubCategoryID", newName: "SubCategory_Code");
            AddColumn("dbo.DataItem", "MainCategoryID", c => c.Int(nullable: false));
            AddColumn("dbo.DataItem", "SubCategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.DataItem", "SubCategory_Code");
            CreateIndex("dbo.DataItem", "MainCategory_Code");
            AddForeignKey("dbo.DataItem", "MainCategory_Code", "dbo.Category", "Code");
            AddForeignKey("dbo.DataItem", "SubCategory_Code", "dbo.Category", "Code");
        }
    }
}
