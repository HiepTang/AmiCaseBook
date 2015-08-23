using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmeCaseBookOrg.Models
{
    public class MainMenu : SubCategory
    {

      
        public ICollection<SubCategory> GetSubMenus()
        {
            return (List<SubCategory>)SubCategories.ToList();

        }
    }
}