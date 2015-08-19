using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.DAL.Infrastructure;

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

        public IEnumerable<MainCategory> GetMainCategories(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MainCategory> GetMainCategories(string UserId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubCategory> GetSubCategories(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubCategory> GetSubCategories(MainCategory mainCategory, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubCategory> GetCountries()
        {
            // First, find the MainCategory Country
            MainCategory countryMainCategory = appContext.MainCategories.Where(c => c.CodeName == "Country").FirstOrDefault();
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
    }
}