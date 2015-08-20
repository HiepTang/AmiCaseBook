using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.DAL.Infrastructure;

namespace AmeCaseBookOrg.Service
{
    public class AnnouncementService : IAnnouncementService
    {
        private ApplicationDbContext context;

        public AnnouncementService(IDbFactory dbFactory)
        {
            context = dbFactory.Init();
        }
        public void CreateAnnouncement(Announcement announcement)
        {
            context.Announcements.Add(announcement);
        }

        public IEnumerable<Announcement> GetAnnouncements()
        {
            return context.Announcements;
        }

        public void SaveAnnouncement()
        {
            context.SaveChanges();
        }
    }
}