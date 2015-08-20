using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace AmeCaseBookOrg.Models
{
    public class File
    {
        public int FileId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }

        public virtual ICollection<Announcement> AttachedToAnnouncements { get; set; }

        public virtual ICollection<CommunityTopic> AttachedToCommunityTopics { get; set; }

        public virtual ICollection<DataItem> AttachedToDataItem { get; set; }

    }
}