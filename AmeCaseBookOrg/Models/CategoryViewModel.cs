using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmeCaseBookOrg.Models
{
    public class CategoryViewModel
    {
        [Key]
        public int Code { get; set; }

        [StringLength(255)]
        [Required]
        public String CodeName { get; set; }

        [StringLength(255)]
        public String URL { get; set; }

        [StringLength(1000)]
        public String Memo { get; set; }
    }
}