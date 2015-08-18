using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmeCaseBookOrg.Models
{
    public class MainMenu : SubCategory
    {

        public ICollection<SubMenu> GetSubMenus()
        {
            return (ICollection<SubMenu>)this.SubCategories;

        }
    }
}