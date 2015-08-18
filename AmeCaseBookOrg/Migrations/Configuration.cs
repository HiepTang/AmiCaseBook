namespace AmeCaseBookOrg.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Web;

    internal sealed class Configuration : DbMigrationsConfiguration<AmeCaseBookOrg.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            List<File> files = createFileData(context);
            List<MainCategory> mainCategory = createMainCategory(context);
            List<SubCategory> subCategory = createSubCategory(context, files);
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    FirstName = "John",
                    LastName = "Can",
                    Id = "john",
                    Email = "join@gmail.com",
                    Introduction = "Hi everyone",
                    CountryId = subCategory[0].Code,
                    LinkIn = "Link in",
                    FileId = files[0].FileId,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "john"
                  
                }
            };
            users.ForEach(u => context.Users.AddOrUpdate(u));
            context.SaveChanges();
            
        }
        private List<File> createFileData(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            //create file table
            var files = new List<File>()
            {
                new File
                {
                    FileName = "b_img.jpg",
                    ContentType = "Image",
                    Content = imageToByteArray( Image.FromFile("D:\\Working\\Project\\Amicasebook\\AmiCaseBook\\AmeCaseBookOrg\\img\\b_img.jpg")),
                    FileType = FileType.Avatar
                 },
                new File
                {
                    FileName = "Belgium.png",
                    ContentType = "Image",
                    Content = imageToByteArray( Image.FromFile("D:\\Working\\Project\\Amicasebook\\AmiCaseBook\\AmeCaseBookOrg\\img\\Belgium.png")),
                    FileType = FileType.Avatar
                 }
            };
            files.ForEach(f => context.Files.AddOrUpdate(f));
            context.SaveChanges();
            return files;
        }
        private List<MainCategory> createMainCategory(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            var mainCategories = new List<MainCategory>()
            {
                new MainCategory
                {
                    Code = 1,
                    CodeName = "Country",
                    IsMenu = true,
                    Memo = "This is memo",
                    URL = "This is url",
                    IsSystemCode = true,

                },
                 new MainCategory
                {
                    Code = 2,
                    CodeName = "Top Menu",
                    IsMenu = true,
                    Memo = "This is memo",
                    URL = "This is url",
                    IsSystemCode = true,

                }
            };
            mainCategories.ForEach(m => context.MainCategories.AddOrUpdate(m));
            context.SaveChanges();
            return mainCategories;
        }
        private List<SubCategory> createSubCategory(AmeCaseBookOrg.Models.ApplicationDbContext context, List<File> files)
        {
            var subCategories = new List<SubCategory>()
            {
                new SubCategory
                {
                    Code = 3,
                    CodeName = "About",
                    IsMenu = true,
                    Memo = "This is memo",
                    URL = "This is url",
                   MainCategoryCode = 1,
                    ImageFileID = files[0].FileId

                },
                 new SubCategory
                {
                    Code = 4,
                    CodeName = "AMI CASE",
                    IsMenu = true,
                    Memo = "This is memo",
                    URL = "This is url",
                   MainCategoryCode= 2,
                    ImageFileID = files[1].FileId
                }
            };
            subCategories.ForEach(m => context.Categories.AddOrUpdate(m));
            context.SaveChanges();
            return subCategories;
        }
        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}
