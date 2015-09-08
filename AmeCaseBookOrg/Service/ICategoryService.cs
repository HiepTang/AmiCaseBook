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
        IEnumerable<MainCategory> GetMainCategories(bool isMenu);

        IEnumerable<MainMenu> GetMainMenus(ApplicationUser user);

        IEnumerable<SubMenu> GetSubMenus();

        IEnumerable<SubMenu> GetDataSubMenus();

        IEnumerable<SubCategory> GetSubMenus(ApplicationUser user);

        IEnumerable<SubCategory> GetSubMenus(ApplicationUser user, MainMenu mainMenu);

        IEnumerable<SubCategory> GetSubCategories(MainCategory mainCategory, ApplicationUser user);

        IEnumerable<SubCategory> GetCountries();

        IEnumerable<MainMenu> GetMainMenus();

        Category GetCategory(int Code);

        void CreateCategory(Category category);

        bool DeleteCategory(Category category);

        void SaveCategory();
    }
}
