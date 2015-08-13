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

        IEnumerable<MainCategory> GetMainCategories(String UserId);



    }
}
