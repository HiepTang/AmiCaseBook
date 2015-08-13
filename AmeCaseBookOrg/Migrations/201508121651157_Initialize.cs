namespace AmeCaseBookOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Announcement",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 1000),
                        Content = c.String(nullable: false, maxLength: 4000),
                        InsertDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        AuthorUserID = c.String(maxLength: 128),
                        LastUpdatedUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorUserID)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedUserID)
                .Index(t => t.AuthorUserID)
                .Index(t => t.LastUpdatedUserID);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Affiliation = c.String(),
                        Introduction = c.String(),
                        LinkIn = c.String(),
                        FileId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.File", t => t.FileId, cascadeDelete: true)
                .Index(t => t.FileId)
                .Index(t => t.CountryId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Code = c.Int(nullable: false),
                        CodeName = c.String(nullable: false, maxLength: 255),
                        URL = c.String(maxLength: 255),
                        Memo = c.String(maxLength: 1000),
                        IsMenu = c.Boolean(nullable: false),
                        ImageFileID = c.Int(),
                        MainCategoryCode = c.Int(),
                        IsSystemCode = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.Category", t => t.MainCategoryCode)
                .ForeignKey("dbo.File", t => t.ImageFileID, cascadeDelete: false)
                .Index(t => t.ImageFileID)
                .Index(t => t.MainCategoryCode);
            
            CreateTable(
                "dbo.DataItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MainCategoryID = c.Int(nullable: false),
                        SubCategoryID = c.Int(nullable: false),
                        CountryID = c.Int(nullable: false),
                        CreatedUserID = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedUserID = c.String(maxLength: 128),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        MainImageID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 1000),
                        Content = c.String(nullable: false, maxLength: 4000),
                        AllowComment = c.Boolean(nullable: false),
                        MainCategory_Code = c.Int(),
                        SubCategory_Code = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Category", t => t.CountryID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedUserID)
                .ForeignKey("dbo.Category", t => t.MainCategory_Code)
                .ForeignKey("dbo.File", t => t.MainImageID, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.SubCategory_Code)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedUserID)
                .Index(t => t.CountryID)
                .Index(t => t.CreatedUserID)
                .Index(t => t.LastUpdatedUserID)
                .Index(t => t.MainImageID)
                .Index(t => t.MainCategory_Code)
                .Index(t => t.SubCategory_Code);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 1000),
                        Name = c.String(nullable: false, maxLength: 255),
                        Email = c.String(nullable: false),
                        ComemmentTime = c.DateTime(nullable: false),
                        DataItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DataItem", t => t.DataItemID, cascadeDelete: true)
                .Index(t => t.DataItemID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(maxLength: 255),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AnnouncementAttachment",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.FileId })
                .ForeignKey("dbo.Announcement", t => t.ID, cascadeDelete: true)
                .ForeignKey("dbo.File", t => t.FileId, cascadeDelete: true)
                .Index(t => t.ID)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.MemberPermission",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.Code })
                .ForeignKey("dbo.AspNetUsers", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.Code, cascadeDelete: false)
                .Index(t => t.Id)
                .Index(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Announcement", "LastUpdatedUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "FileId", "dbo.File");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DataItem", "CreatedUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CountryId", "dbo.Category");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MemberPermission", "Code", "dbo.Category");
            DropForeignKey("dbo.MemberPermission", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Category", "ImageFileID", "dbo.File");
            DropForeignKey("dbo.DataItem", "SubCategory_Code", "dbo.Category");
            DropForeignKey("dbo.DataItem", "MainImageID", "dbo.File");
            DropForeignKey("dbo.DataItem", "MainCategory_Code", "dbo.Category");
            DropForeignKey("dbo.Category", "MainCategoryCode", "dbo.Category");
            DropForeignKey("dbo.DataItem", "LastUpdatedUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.DataItem", "CountryID", "dbo.Category");
            DropForeignKey("dbo.Comment", "DataItemID", "dbo.DataItem");
            DropForeignKey("dbo.Announcement", "AuthorUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnnouncementAttachment", "FileId", "dbo.File");
            DropForeignKey("dbo.AnnouncementAttachment", "ID", "dbo.Announcement");
            DropIndex("dbo.MemberPermission", new[] { "Code" });
            DropIndex("dbo.MemberPermission", new[] { "Id" });
            DropIndex("dbo.AnnouncementAttachment", new[] { "FileId" });
            DropIndex("dbo.AnnouncementAttachment", new[] { "ID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Comment", new[] { "DataItemID" });
            DropIndex("dbo.DataItem", new[] { "SubCategory_Code" });
            DropIndex("dbo.DataItem", new[] { "MainCategory_Code" });
            DropIndex("dbo.DataItem", new[] { "MainImageID" });
            DropIndex("dbo.DataItem", new[] { "LastUpdatedUserID" });
            DropIndex("dbo.DataItem", new[] { "CreatedUserID" });
            DropIndex("dbo.DataItem", new[] { "CountryID" });
            DropIndex("dbo.Category", new[] { "MainCategoryCode" });
            DropIndex("dbo.Category", new[] { "ImageFileID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CountryId" });
            DropIndex("dbo.AspNetUsers", new[] { "FileId" });
            DropIndex("dbo.Announcement", new[] { "LastUpdatedUserID" });
            DropIndex("dbo.Announcement", new[] { "AuthorUserID" });
            DropTable("dbo.MemberPermission");
            DropTable("dbo.AnnouncementAttachment");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Country");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Comment");
            DropTable("dbo.DataItem");
            DropTable("dbo.Category");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.File");
            DropTable("dbo.Announcement");
        }
    }
}
