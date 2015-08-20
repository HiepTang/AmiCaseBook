using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace AmeCaseBookOrg.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [AllowHtml]
        [Required]
        [MaxLength(1000)]
        public String Content { get; set; }

        [Required]
        [MaxLength(255)]
        public String Name { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        public DateTime ComemmentTime { get; set; }

        public int DataItemID { get; set; }

        [ForeignKey("DataItemID")]
        public virtual DataItem DataItem { get; set; }

        



    }
}