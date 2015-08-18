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
        
    }
}