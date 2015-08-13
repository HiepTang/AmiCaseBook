using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AmeCaseBookOrg.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Code { get; set; }

        [StringLength(255)]
        [Required]
        public String CodeName { get; set; }

        [StringLength(255)]
        public String URL { get; set; }

        [StringLength(1000)]
        public String Memo { get; set; }

        [Display(Name ="Menu")]
        public Boolean IsMenu { get; set; }
        
    }
}