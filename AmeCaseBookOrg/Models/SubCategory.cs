using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmeCaseBookOrg.Models
{
    public class SubCategory : Category
    {

        public int? ImageFileID { get; set; }


        [ForeignKey("ImageFileID")]
        public virtual File Image { get; set; }


        public int ParentCategoryCode { get; set; }

        [ForeignKey("ParentCategoryCode")]
        public virtual Category ParentCategory { get; set; }

    }
}