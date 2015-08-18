namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMenuDefinitionClasses1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Category", "ImageFileID", "dbo.File");
            AddForeignKey("dbo.Category", "ImageFileID", "dbo.File", "FileId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Category", "ImageFileID", "dbo.File");
            AddForeignKey("dbo.Category", "ImageFileID", "dbo.File", "FileId", cascadeDelete: true);
        }
    }
}
