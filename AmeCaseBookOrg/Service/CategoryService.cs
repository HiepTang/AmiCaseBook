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
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetCategories()
        {
            return appContext.Categories.ToList();
        }

        public Category GetCategory(int Code)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MainCategory> GetMainCategories()
        {
            return appContext.MainCategories.ToList();
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

        public void SaveCategory()
        {
            throw new NotImplementedException();
        }
    }
}