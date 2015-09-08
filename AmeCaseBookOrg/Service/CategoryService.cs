using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.DAL.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace AmeCaseBookOrg.Service
{
    public class CategoryService : ICategoryService
    {
        private ApplicationDbContext appContext;
        public CategoryService(IDbFactory dbFactory)
        {
            this.appContext = dbFactory.Init();
        }

        public void CreateCategory(Category category)
        {
            appContext.Categories.Add(category);
        }

        public IEnumerable<Category> GetCategories()
        {
            return appContext.Categories.ToList();
        }

        public Category GetCategory(int Code)
        {
            var category = appContext.Categories.Find(Code);
            return category;
        }

        public IEnumerable<MainCategory> GetMainCategories()
        {
            return appContext.MainCategories;
        }
        public IEnumerable<MainCategory> GetMainCategories(bool isMenu)
        {
            return appContext.MainCategories.Where(m => m.IsMenu == isMenu);
        }
        public IEnumerable<MainMenu> GetMainMenus(ApplicationUser user)
        {
            var subMenus = user.CanAccessCategories;
            List<MainMenu> mainMenus = new List<MainMenu>();
            if( null != subMenus)
            {
                foreach (var subMenu in subMenus)
                {
                    MainMenu mainMenu = (MainMenu)subMenu.ParentCategory;
                    if (!mainMenus.Contains(mainMenu))
                    {
                        mainMenus.Add(mainMenu);
                    }
                }
            }
            return mainMenus;
        }

        public IEnumerable<SubCategory> GetSubMenus(ApplicationUser user, MainMenu mainMenu)
        {
            List<SubCategory> subMenus = new List<SubCategory>();

            if(null != user.CanAccessCategories)
            {
                // get all submenus of mainmenu
                List<SubCategory> subAllMenus = mainMenu.GetSubMenus().ToList();
                // all submenus have grant permission to user
                List<SubMenu> permissionSubMenus = user.CanAccessCategories.ToList();

                foreach (var subMenu in subAllMenus)
                {
                    if (permissionSubMenus.Contains(subMenu))
                    {
                        subMenus.Add(subMenu);
                    }
                }
            }

            return subMenus;
        }

        public IEnumerable<MainCategory> GetMainCategories(string UserId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubCategory> GetSubMenus(ApplicationUser user)
        {
            return user.CanAccessCategories;
            
        }

        public IEnumerable<SubCategory> GetSubCategories(MainCategory mainCategory, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubCategory> GetCountries()
        {
            // First, find the MainCategory Country
            MainCategory countryMainCategory = appContext.MainCategories.Where(c => c.Code == (int) MainCategoryType.Country).FirstOrDefault();
            IEnumerable<SubCategory> countries = null;
            if(countryMainCategory != null)
            {
                countries = countryMainCategory.SubCategories;
            }
            return countries;
        }

        public IEnumerable<MainMenu> GetMainMenus()
        {
            return appContext.MainMenus;
        }
        public void SaveCategory()
        {
            appContext.SaveChanges();
        }

        public IEnumerable<SubMenu> GetSubMenus()
        {
          return appContext.SubMenus;
        }

        public IEnumerable<SubMenu> GetDataSubMenus()
        {
            return appContext.SubMenus.Where(m => m.ParentCategory.URL == "DataItem/List");
        }
        public bool DeleteCategory(Category category)
        {
            if(category is SubMenu)
            {
                SubMenu subMenu = category as SubMenu;
                if (subMenu.DataItems.Count > 0)
                    return false;
            }
            appContext.Categories.Remove(category);
            return true;
        }
    }
}