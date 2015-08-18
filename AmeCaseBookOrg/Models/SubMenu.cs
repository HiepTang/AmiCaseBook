using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmeCaseBookOrg.Models
{
    public class SubMenu : SubCategory
    {

        public MainMenu GetMainMenu()
        {
            return (MainMenu)this.ParentCategory;
        }
    }
}