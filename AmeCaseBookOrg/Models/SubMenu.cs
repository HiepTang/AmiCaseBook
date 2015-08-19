using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmeCaseBookOrg.Models
{
    public class SubMenu : SubCategory
    {

        public MainMenu GetMainMenu()
        {
            return (MainMenu)this.ParentCategory;
        }


        public virtual ICollection<ApplicationUser> GrantForUsers { get; set; }

        [InverseProperty("SubCategory")]
        public virtual ICollection<DataItem> DataItems { get; set; }
    }
}