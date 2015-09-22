using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace AmeCaseBookOrg.Models
{
    public class DataItem
    {

        public int ID { get; set; }

        public int MainMenuID { get; set; }
        [Required]
        public int SubCategoryID { get; set; }

        public int CountryID { get; set; }


        public string CreatedUserID { get; set; }



        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        public String LastUpdatedUserID { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        

        [Required]
        [MaxLength(1000)]
        public string Title { get; set; }

        [AllowHtml]
        [Required]
        [MaxLength, Column(TypeName = "ntext")]
        public string Content { get; set; }
        public bool AllowComment { get; set; }

        [ForeignKey("MainMenuID")]
        public virtual MainMenu MainMenu { get; set; }
        
        [ForeignKey("SubCategoryID")]
        public virtual SubMenu SubCategory { get; set; }
        
        [ForeignKey("CountryID")]
        public virtual SubCategory Country { get; set; }
        
        public virtual ICollection<File> Images { get; set; }

        public virtual ICollection<File> AttachFiles { get; set; }

        [ForeignKey("CreatedUserID")]
        public virtual ApplicationUser CreatedUser { get; set; }
        
        [ForeignKey("LastUpdatedUserID")]
        public virtual ApplicationUser LastUpdatedUser { get; set; }

        [InverseProperty("DataItem")]
        public virtual ICollection<Comment> Comments { get; set; }

    }
}