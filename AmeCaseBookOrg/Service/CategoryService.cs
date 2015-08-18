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
        public IEnumerable<Category> GetCategories()
        {
            return appContext.Categories.ToList();
        }

        public IEnumerable<MainCategory> GetMainCategories()
        {
            return appContext.MainCategories.ToList();
        }

        public IEnumerable<MainCategory> GetMainCategories(string UserId)
        {
            throw new NotImplementedException();
        }
    }
}