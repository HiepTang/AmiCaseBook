namespace AmeCaseBookOrg.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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
        //string imageFolder = "D:\\Working\\Project\\Amicasebook\\AmiCaseBook\\AmeCaseBookOrg\\img\\";
        //string imageFolder = "D:\\Working\\AmiCaseBook\\Source\\AmiCaseBook\\AmeCaseBookOrg\\img\\";
        string imageFolder = "C:\\Working\\Amicasebook\\Source\\AmeCaseBookOrg\\img\\";
        protected override void Seed(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            List<File> memberImages = CreateMemberImages(context);
            List<MainCategory> mainCategory = createMainCategory(context);
            List<File> countryFlagFiles = CreateFlagImages(context);
            List<SubCategory> countries = CreateCountries(context, countryFlagFiles);
            List<MainMenu> mainMenus = CreateMainMenus(context);
            List<SubMenu> subMenus = CreateSubMenusForAbout(context);
            List<SubMenu> subMenusForAMI = CreateSubMenusForAMICASE(context, countries);
            List<SubMenu> subMenusForDSM = CreateSubMenusForDSMCASE(context, countries);
            List<SubMenu> subMenusForCommunication = CreateSubMenusForCommunication(context);
           
            
            List<ApplicationUser> users = CreateMembers(context, memberImages, countries);

            List<File> dataItemImages = CreateDataItemImages(context);
            List<DataItem> amiDataItems = CreateDataItems(context, subMenusForAMI, countries, users, dataItemImages);
            List<DataItem> dsmDataItems = CreateDataItems(context, subMenusForDSM, countries, users, dataItemImages);
            List<Announcement> announcements = CreateAnnouncements(context, subMenusForCommunication, users);
            List<CommunityTopic> communityTopics = CreateCommunityTopic(context, subMenusForCommunication, users);
        }

        private List<Announcement> CreateAnnouncements(ApplicationDbContext context, List<SubMenu> subMenus, List<ApplicationUser> members)
        {
            List<Announcement> announcements = new List<Announcement>();
            for (int i = 0; i < 20; i++)
            {
                Announcement announcement = new Announcement {
                    Title = "Announcement title " + i,
                    Content = "Announcement content " + 1,
                    InsertDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    AuthorUserID = members[4 + (i % 2)].Id,
                    LastUpdatedUserID = members[4 + (i % 2)].Id
                };
                announcements.Add(announcement);
            }
            announcements.ForEach(f => context.Announcements.AddOrUpdate(f));
            context.SaveChanges();

            return announcements;
        }

        private List<CommunityTopic> CreateCommunityTopic(ApplicationDbContext context, List<SubMenu> subMenus, List<ApplicationUser> members)
        {
            List<CommunityTopic> communityTopics = new List<CommunityTopic>();
            for (int i = 0; i < 20; i++)
            {
                CommunityTopic communityTopic = new CommunityTopic
                {
                    Title = "Community Topic title " + i,
                    Content = "Community Topic content " + 1,
                    InsertDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    AuthorUserID = members[4 + (i % 2)].Id,
                    LastUpdatedUserID = members[4 + (i % 2)].Id
                };
                communityTopics.Add(communityTopic);
            }
            communityTopics.ForEach(f => context.CommunityTopics.AddOrUpdate(f));
            context.SaveChanges();

            return communityTopics;
        }


        private List<DataItem> CreateDataItems(AmeCaseBookOrg.Models.ApplicationDbContext context, List<SubMenu> subMenus, List<SubCategory> countries, List<ApplicationUser> members, List<File> dataItemImages)
        {
            List<DataItem> dataItems = new List<DataItem>();

            for (int i = 0; i < subMenus.Count; i++)
            {
                SubMenu subMenu = subMenus[i];
                SubCategory country = countries[13]; // Korea for Key Findings
                if(i > 0)
                {
                    country = countries[i - 1];
                }

                ApplicationUser member = members[4];
                if(i % 2 == 0)
                {
                    member = members[5];
                }

                DataItem dataItem = new DataItem
                {
                    MainMenuID = subMenu.ParentCategoryCode,
                    SubCategoryID = subMenu.Code,
                    CountryID = country.Code,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    CreatedUserID = member.Id,
                    LastUpdatedUserID = member.Id,
                    Title = i.ToString() + ". " + country.CodeName.ToUpper(),
                    Content = "<p>It is the sample content for CASE</p>",
                    AllowComment = true    
                };
                List<File> image = new List<File> {dataItemImages[i % 3] };
                dataItem.Images = image;
                dataItems.Add(dataItem);
            }

            dataItems.ForEach(f => context.DataItems.AddOrUpdate(f));
            context.SaveChanges();

            return dataItems;

        }
        private List<ApplicationUser> CreateMembers(AmeCaseBookOrg.Models.ApplicationDbContext context, List<File> memberImages, List<SubCategory> countries)
        {
            if (!context.Roles.Any(u => u.Name == "Contributor"))
            {
                context.Roles.Add(new IdentityRole { Name = "Contributor" });
            }

            if (!context.Roles.Any(u => u.Name == "Admin"))
            {
                context.Roles.Add(new IdentityRole { Name = "Admin" });
            }


            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("123456");

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    FirstName = "Dong",
                    LastName = "Joo Kang",
                    Email = "dong.joo.kang@gmail.com",
                    Introduction = "Dong Joo Kang",
                    CountryId = countries[13].Code,
                    LinkIn = "Link in",
                    FileId = memberImages[0].FileId,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "dong.joo.kang@gmail.com",
                    PasswordHash = password,
                    PhoneNumber = "84903666892"

                },
                 new ApplicationUser
                {
                    FirstName = "Laura",
                    LastName = "Marretta",
                    Email = "laura.marretta@gmail.com",
                    Introduction = "Laura Marretta",
                    CountryId = countries[12].Code,
                    LinkIn = "Laura Marretta Link in",
                    FileId = memberImages[1].FileId,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "laura.marretta@gmail.com",
                    PasswordHash = password,
                    PhoneNumber = "84903666892"
                },
                 new ApplicationUser
                {
                    FirstName = "Jon",
                    LastName = "Stromsather",
                    Email = "jon.stromsather@gmail.com",
                    Introduction = "Jon Stromsather",
                    CountryId = countries[12].Code,
                    LinkIn = "Jon Stromsather Link in",
                    FileId = memberImages[2].FileId,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "jon.stromsather@gmail.com",
                    PasswordHash = password,
                    PhoneNumber = "84903666892"
                },
                 new ApplicationUser
                {
                    FirstName = "Loc",
                    LastName = "Truong",
                    Email = "locitt@gmail.com",
                    Introduction = "Loc Truong",
                    CountryId = countries[13].Code,
                    LinkIn = "Loc Truong",
                    FileId = memberImages[0].FileId,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "locitt@gmail.com",
                    PasswordHash = password,
                    PhoneNumber = "84903666892"
                },
                  new ApplicationUser
                {
                    FirstName = "Hiep",
                    LastName = "Tang",
                    Email = "tpthiep@gmail.com",
                    Introduction = "Hiep Tang",
                    CountryId = countries[3].Code,
                    LinkIn = "Hiep Tang",
                    FileId = memberImages[0].FileId,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "tpthiep@gmail.com",
                    PasswordHash = password,
                    PhoneNumber = "84903666892"
                },
                   new ApplicationUser
                {
                    FirstName = "Phuc",
                    LastName = "Nguyen",
                    Email = "phuc0903@gmail.com",
                    Introduction = "Phuc Nguyen",
                    CountryId = countries[12].Code,
                    LinkIn = "Phuc Nguyen",
                    FileId = memberImages[0].FileId,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "phuc0903@gmail.com",
                    PasswordHash = password,
                    PhoneNumber = "84903666892"
                }
            };
            users.ForEach(u => userManager.Create(u));
            var newUsers = context.Users.ToList();
            ApplicationUser user = newUsers.Find(u => u.UserName == "phuc0903@gmail.com");
            userManager.AddToRole(user.Id, "Admin");

            user = newUsers.Find(u => u.UserName == "tpthiep@gmail.com");
            userManager.AddToRole(user.Id, "Admin");

            user = newUsers.Find(u => u.UserName == "locitt@gmail.com");
            userManager.AddToRole(user.Id, "Admin");

            user = newUsers.Find(u => u.UserName == "jon.stromsather@gmail.com");
            userManager.AddToRole(user.Id, "Contributor");

            user = newUsers.Find(u => u.UserName == "laura.marretta@gmail.com");
            userManager.AddToRole(user.Id, "Contributor");

            user = newUsers.Find(u => u.UserName == "dong.joo.kang@gmail.com");
            userManager.AddToRole(user.Id, "Contributor");

            return users;
        }

        private List<File> CreateFlagImages(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            
            var files = new List<File>()
            {
                CreateFile("Austria.png", imageFolder, FileType.Flag),
                CreateFile("Australia.png", imageFolder, FileType.Flag),
                CreateFile("Belgium.png", imageFolder, FileType.Flag),
                CreateFile("Canada.png", imageFolder, FileType.Flag),
                CreateFile("China.png", imageFolder, FileType.Flag),
                CreateFile("Denmark.png", imageFolder, FileType.Flag),
                CreateFile("EuropeanCommission.png", imageFolder, FileType.Flag),
                CreateFile("Finland.png", imageFolder, FileType.Flag),
                CreateFile("France.png", imageFolder, FileType.Flag),
                CreateFile("Germany.png", imageFolder, FileType.Flag),
                CreateFile("India.png", imageFolder, FileType.Flag),
                CreateFile("Ireland.png", imageFolder, FileType.Flag),
                CreateFile("Italy.png", imageFolder, FileType.Flag),
                CreateFile("Korea.png", imageFolder, FileType.Flag),
                CreateFile("Mexico.png", imageFolder, FileType.Flag),
                CreateFile("Norway.png", imageFolder, FileType.Flag),
                CreateFile("Russia.png", imageFolder, FileType.Flag),
                CreateFile("Singapore.png", imageFolder, FileType.Flag),
                CreateFile("South_Africa.png", imageFolder, FileType.Flag),
                CreateFile("Spain.png", imageFolder, FileType.Flag),
                CreateFile("Sweden.png", imageFolder, FileType.Flag),
                CreateFile("Switzerland.png", imageFolder, FileType.Flag),
                CreateFile("Netherlands.png", imageFolder, FileType.Flag),
                CreateFile("United_States.png", imageFolder, FileType.Flag)

            };
            files.ForEach(f => context.Files.AddOrUpdate(f));
            context.SaveChanges();
            return files;
        }

        private File CreateFile(string fileName, string imageFolder, FileType fileType)
        {
            File file = new File {
                FileName = fileName,
                ContentType = "Image",
                Content = imageToByteArray(Image.FromFile(imageFolder + fileName)),
                FileType = fileType
            };
            return file;
        }
        private List<File> CreateMemberImages(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            //create file table
            var files = new List<File>()
            {
               CreateFile("Dong Joo Kang.jpg", imageFolder, FileType.Avatar),
               CreateFile("Laura Marretta.jpg", imageFolder, FileType.Avatar),
               CreateFile("Jon Stromsather.jpg", imageFolder, FileType.Avatar)
            };
            files.ForEach(f => context.Files.AddOrUpdate(f));
            context.SaveChanges();
            return files;
        }

        private List<File> CreateDataItemImages(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            //create file table
            var files = new List<File>()
            {
               CreateFile("Korea1-640x450.jpg", imageFolder, FileType.Avatar),
               CreateFile("SPAIN1-640x4502-270x250.png", imageFolder, FileType.Avatar),
               CreateFile("France-640x450-270x250.jpg", imageFolder, FileType.Avatar),
               CreateFile("A-summary.png", imageFolder, FileType.Avatar),
               CreateFile("img01.png", imageFolder, FileType.Avatar)
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
                    Code = (int)MainCategoryType.Country,
                    CodeName = "Country",
                    IsMenu = false,
                    Memo = "",
                    URL = "",
                    IsSystemCode = true,

                },
                 new MainCategory
                {
                    Code = (int)MainCategoryType.Menu,
                    CodeName = "Menu",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                    IsSystemCode = true,

                }
            };
            mainCategories.ForEach(m => context.MainCategories.AddOrUpdate(m));
            context.SaveChanges();
            return mainCategories;
        }
        private List<MainMenu> CreateMainMenus(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            var mainMenus = new List<MainMenu>()
            {
                new MainMenu
                {
                    Code = 3001001,
                    CodeName = "ABOUT",
                    IsMenu = true,
                    Memo = "",
                    URL = "About/Index",
                   ParentCategoryCode = 103001

                },
                 new MainMenu
                {
                    Code = 3001002,
                    CodeName = "AMI CASE",
                    IsMenu = true,
                    Memo = "",
                    URL = "DataItem/List",
                   ParentCategoryCode= 103001
                  
                },
                 new MainMenu
                {
                    Code = 3001003,
                    CodeName = "DSM CASE",
                    IsMenu = true,
                    Memo = "",
                    URL = "DataItem/List",
                   ParentCategoryCode= 103001
                },
                 new MainMenu
                {
                    Code = 3001004,
                    CodeName = "Communication",
                    IsMenu = true,
                    Memo = "",
                    URL = "Communication/Index",
                   ParentCategoryCode= 103001
                    
                }
            };
            mainMenus.ForEach(m => context.Categories.AddOrUpdate(m));
            context.SaveChanges();
            return mainMenus;
        }

        private List<SubCategory> CreateCountries(AmeCaseBookOrg.Models.ApplicationDbContext context, List<File> countryImageFiles)
        {
            List<SubCategory> countries = new List<SubCategory>();
            int beginCode = 1020010;
            foreach (File file in countryImageFiles)
            {
                string countryName = file.FileName.Substring(0, file.FileName.Length - 4);
                SubCategory country = new SubCategory
                {
                    Code = beginCode + 1,
                    CodeName = countryName,
                    IsMenu = false,
                    Memo = countryName,
                    URL = "",
                    ParentCategoryCode = 102001,
                    ImageFileID = file.FileId
                };
                beginCode = beginCode + 1;
                countries.Add(country);
            }

            countries.ForEach(m => context.Categories.AddOrUpdate(m));
            context.SaveChanges();
            return countries;
        }

        private List<SubMenu> CreateSubMenusForAbout(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            var subMenus = new List<SubMenu>()
            {
                new SubMenu
                {
                    Code = 30010011,
                    CodeName = "Casebook",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode = 3001001

                },
                 new SubMenu
                {
                    Code = 30010012,
                    CodeName = "ISGAN",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001001

                },
                 new SubMenu
                {
                    Code = 30010013,
                    CodeName = "CONTRIBUTORS",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001001
                }
                
            };
            subMenus.ForEach(m => context.Categories.AddOrUpdate(m));
            context.SaveChanges();
            return subMenus;
        }

        private List<SubMenu> CreateSubMenusForAMICASE(AmeCaseBookOrg.Models.ApplicationDbContext context, List<SubCategory> countries)
        {
            var subMenus = new List<SubMenu>()
            {
                new SubMenu
                {
                    Code = 30010021,
                    CodeName = "Key Findings",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode = 3001002

                }
            };

            int beginCode = 30010021;
            int step = 1;
            foreach (var country in countries)
            {
                if(step == 9){
                    step = 270090189;
                }
                SubMenu subMenu = new SubMenu
                {
                    Code = beginCode + step,
                    CodeName = country.CodeName,
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                    ParentCategoryCode = 3001002
                };
                step= step + 1;
                subMenus.Add(subMenu);
            }
            subMenus.ForEach(m => context.Categories.AddOrUpdate(m));
            context.SaveChanges();
            return subMenus;
        }

        private List<SubMenu> CreateSubMenusForDSMCASE(AmeCaseBookOrg.Models.ApplicationDbContext context, List<SubCategory> countries)
        {
            var subMenus = new List<SubMenu>()
            {
                new SubMenu
                {
                    Code = 30010031,
                    CodeName = "Key Findings",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode = 3001003

                }
            };
            int beginCode = 30010031;
            int step = 1;
            foreach (var country in countries)
            {
                if (step == 9) step = 270090279;
                SubMenu subMenu = new SubMenu
                {
                    Code = beginCode + step,
                    CodeName = country.CodeName,
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                    ParentCategoryCode = 3001003
                };
                step = step + 1;
                subMenus.Add(subMenu);
            }
            subMenus.ForEach(m => context.Categories.AddOrUpdate(m));
            context.SaveChanges();
            return subMenus;
        }
        private List<SubMenu> CreateSubMenusForCommunication(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            var subMenus = new List<SubMenu>()
            {
                new SubMenu
                {
                    Code = 30010041,
                    CodeName = "Announcements",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode = 3001004

                },
                 new SubMenu
                {
                    Code = 30010042,
                    CodeName = "Community",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001004

                }

            };
            subMenus.ForEach(m => context.Categories.AddOrUpdate(m));
            context.SaveChanges();
            return subMenus;
        }
        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}
