using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AmeCaseBookOrg.Models
{
    public class CommuityTopicComment
    {


        public int ID { get; set; }

        [Required]
        [MaxLength(1000)]
        public String Content { get; set; }

        public DateTime ComemmentTime { get; set; }

        public String CommentUserId { get; set; }

        public int CommunityTopicID { get; set; }

        [ForeignKey("CommentUserId")]
        public virtual ApplicationUser CommentUser { get; set; }

        [ForeignKey("CommunityTopicID")]
        public virtual CommunityTopic CommunityTopic { get; set; }


    }
}