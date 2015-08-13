using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AmeCaseBookOrg.Models
{
    public class Country
    {

        public int CountryId { get; set; }
        [StringLength(255)]
        public string CountryName { get; set; }

        public int SortOrder { get; set; }

    }
}