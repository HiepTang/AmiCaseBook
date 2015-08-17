using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmeCaseBookOrg.Models;


namespace AmeCaseBookOrg.Service
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();

        IEnumerable<MainCategory> GetMainCategories();

        IEnumerable<MainCategory> GetMainCategories(ApplicationUser user);

        IEnumerable<SubCategory> GetSubCategories(ApplicationUser user);

        IEnumerable<SubCategory> GetSubCategories(MainCategory mainCategory, ApplicationUser user);


        Category GetCategory(int Code);

        void CreateCategory(Category category);

        void SaveCategory();
    }
}
