namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataItemModelsModified : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DataItem", "MainImageID", "dbo.File");
            DropIndex("dbo.DataItem", new[] { "MainImageID" });
            CreateTable(
                "dbo.DataItemImage",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.FileId })
                .ForeignKey("dbo.DataItem", t => t.ID, cascadeDelete: true)
                .ForeignKey("dbo.File", t => t.FileId, cascadeDelete: true)
                .Index(t => t.ID)
                .Index(t => t.FileId);
            
            DropColumn("dbo.DataItem", "MainImageID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataItem", "MainImageID", c => c.Int(nullable: false));
            DropForeignKey("dbo.DataItemImage", "FileId", "dbo.File");
            DropForeignKey("dbo.DataItemImage", "ID", "dbo.DataItem");
            DropIndex("dbo.DataItemImage", new[] { "FileId" });
            DropIndex("dbo.DataItemImage", new[] { "ID" });
            DropTable("dbo.DataItemImage");
            CreateIndex("dbo.DataItem", "MainImageID");
            AddForeignKey("dbo.DataItem", "MainImageID", "dbo.File", "FileId", cascadeDelete: true);
        }
    }
}
