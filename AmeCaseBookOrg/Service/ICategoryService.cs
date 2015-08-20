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

        IEnumerable<MainMenu> GetMainMenus(ApplicationUser user);

        IEnumerable<SubCategory> GetSubMenus(ApplicationUser user);

        IEnumerable<SubCategory> GetSubMenus(ApplicationUser user, MainMenu mainMenu);

        IEnumerable<SubCategory> GetSubCategories(MainCategory mainCategory, ApplicationUser user);

        IEnumerable<SubCategory> GetCountries();

        IEnumerable<MainMenu> GetMainMenus();

        Category GetCategory(int Code);

        void CreateCategory(Category category);

        void SaveCategory();
    }
}
