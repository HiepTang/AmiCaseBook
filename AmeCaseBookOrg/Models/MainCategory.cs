using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmeCaseBookOrg.Models
{
    public class MainCategory : Category
    {

        [Display(Name ="System Code")]
        public Boolean IsSystemCode { get; set; }

        [InverseProperty("MainCategory")]
        public virtual ICollection<SubCategory> SubCategories { get; set; }

        
    }
}