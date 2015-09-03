using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmeCaseBookOrg.Models
{
    public class DataItemViewModel
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(1000)]
        public String Title { get; set; }

        [AllowHtml]
        [Required]
        [MaxLength(4000)]
        public String Content { get; set; }
        [Required]
        public int? SubCategoryID { get; set; }

        public int CountryID { get; set; }

        public virtual ICollection<File> Images { get; set; }

        public virtual ICollection<File> AttachFiles { get; set; }

        public string ReturnedURL { get; set; }
    }
}