using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace AmeCaseBookOrg.Models
{
    public class Announcement
    {

        public int ID { get; set; }

        [Required]
        [MaxLength(1000)]
        public String Title { get; set; }

        [AllowHtml]
        [Required]
        [MaxLength(4000)]
        public String Content { get; set; }

        [Display(Name = "Insert date")]
        public DateTime InsertDate { get; set; }

        [Display(Name = "Edit date")]
        public DateTime LastUpdatedDate { get; set; }


        [Display(Name = "Author")]
        public String AuthorUserID { get; set; }

        public String LastUpdatedUserID { get; set; }

        public virtual ICollection<File> AttachmentFiles { get; set; }
        
        [ForeignKey("AuthorUserID")]
        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("LastUpdatedUserID")]
        public virtual ApplicationUser LastUpdatedUser { get; set; }

    }
}