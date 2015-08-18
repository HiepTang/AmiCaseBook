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
        string imageFolder = "D:\\Working\\Project\\Amicasebook\\AmiCaseBook\\AmeCaseBookOrg\\img\\";
        protected override void Seed(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            List<File> files = createFileData(context);
            List<MainCategory> mainCategory = createMainCategory(context);
            List<MainMenu> mainMenus = CreateMainMenus(context, files);
            List<SubMenu> subMenus = CreateSubMenusForAbout(context);
            List<SubMenu> subMenusForAMI = CreateSubMenusForAMICASE(context);
            List<SubMenu> subMenusForDSM = CreateSubMenusForDSMCASE(context);
            List<SubMenu> subMenusForCommunication = CreateSubMenusForCommunication(context);
            List<File> countryFlagFiles = CreateFlagImages(context);
            List<SubCategory> countries = CreateCountries(context, countryFlagFiles);


            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    FirstName = "John",
                    LastName = "Can",
                    Id = "john",
                    Email = "join@gmail.com",
                    Introduction = "Hi everyone",
                    CountryId = mainMenus[0].Code,
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
        private List<File> createFileData(AmeCaseBookOrg.Models.ApplicationDbContext context)
        {
            
            //create file table
            var files = new List<File>()
            {
                new File
                {
                    FileName = "b_img.jpg",
                    ContentType = "Image",
                    Content = imageToByteArray( Image.FromFile(imageFolder+ "b_img.jpg")),
                    FileType = FileType.Avatar
                 },
                new File
                {
                    FileName = "Belgium.png",
                    ContentType = "Image",
                    Content = imageToByteArray( Image.FromFile(imageFolder+"Belgium.png")),
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
                    Code = 102001,
                    CodeName = "Country",
                    IsMenu = false,
                    Memo = "",
                    URL = "",
                    IsSystemCode = true,

                },
                 new MainCategory
                {
                    Code = 103001,
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
        private List<MainMenu> CreateMainMenus(AmeCaseBookOrg.Models.ApplicationDbContext context, List<File> files)
        {
            var mainMenus = new List<MainMenu>()
            {
                new MainMenu
                {
                    Code = 3001001,
                    CodeName = "ABOUT",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode = 103001

                },
                 new MainMenu
                {
                    Code = 3001002,
                    CodeName = "AMI CASE",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 103001
                  
                },
                 new MainMenu
                {
                    Code = 3001003,
                    CodeName = "DSM CASE",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 103001
                },
                 new MainMenu
                {
                    Code = 3001004,
                    CodeName = "Communication",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
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

        private List<SubMenu> CreateSubMenusForAMICASE(AmeCaseBookOrg.Models.ApplicationDbContext context)
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

                },
                 new SubMenu
                {
                    Code = 30010022,
                    CodeName = "Austria",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001002

                },
                 new SubMenu
                {
                    Code = 30010023,
                    CodeName = "Canada",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001002
                },
                 new SubMenu
                {
                    Code = 30010024,
                    CodeName = "France",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode = 3001002

                },
                 new SubMenu
                {
                    Code = 30010025,
                    CodeName = "Ireland",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001002

                },
                 new SubMenu
                {
                    Code = 30010026,
                    CodeName = "Italy",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001002
                },
                 new SubMenu
                {
                    Code = 30010027,
                    CodeName = "Korea",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001002

                },
                 new SubMenu
                {
                    Code = 30010028,
                    CodeName = "Netherlands",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001002
                },
                 new SubMenu
                {
                    Code = 30010029,
                    CodeName = "Sweden",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode = 3001002

                },
                 new SubMenu
                {
                    Code = 300100210,
                    CodeName = "USA",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001002

                },
                 new SubMenu
                {
                    Code = 300100211,
                    CodeName = "Spain",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001002
                }

            };
            subMenus.ForEach(m => context.Categories.AddOrUpdate(m));
            context.SaveChanges();
            return subMenus;
        }

        private List<SubMenu> CreateSubMenusForDSMCASE(AmeCaseBookOrg.Models.ApplicationDbContext context)
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
                   ParentCategoryCode = 3001003

                },
                 new SubMenu
                {
                    Code = 30010022,
                    CodeName = "Austria",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001003

                },
                 new SubMenu
                {
                    Code = 30010023,
                    CodeName = "Canada",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001003
                },
                 new SubMenu
                {
                    Code = 30010024,
                    CodeName = "France",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode = 3001003

                },
                 new SubMenu
                {
                    Code = 30010025,
                    CodeName = "Ireland",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001003

                },
                 new SubMenu
                {
                    Code = 30010026,
                    CodeName = "Italy",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001003
                },
                 new SubMenu
                {
                    Code = 30010027,
                    CodeName = "Korea",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001003

                },
                 new SubMenu
                {
                    Code = 30010028,
                    CodeName = "Netherlands",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001003
                },
                 new SubMenu
                {
                    Code = 30010029,
                    CodeName = "Sweden",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode = 3001003

                },
                 new SubMenu
                {
                    Code = 300100210,
                    CodeName = "USA",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001003

                },
                 new SubMenu
                {
                    Code = 300100211,
                    CodeName = "Spain",
                    IsMenu = true,
                    Memo = "",
                    URL = "",
                   ParentCategoryCode= 3001003
                }

            };
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
                    Code = 3001004,
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
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}
