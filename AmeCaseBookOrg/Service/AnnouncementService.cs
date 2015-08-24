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

        public Announcement GetAnnouncement(int id)
        {
            return this.context.Announcements.Find(id);
        }

        public IEnumerable<Announcement> GetAnnouncements()
        {
            return context.Announcements;
        }

        public void SaveAnnouncement()
        {
            context.SaveChanges();
        }

        public IEnumerable<Announcement> Search(AnnouncementSearchFilter filter, string sortColumn, string sortOrder, int pageSize, int pageIndex, out int totalRecords)
        {
            IEnumerable<Announcement> anns = context.Announcements;


            if (!string.IsNullOrEmpty(filter.Title))
            {
                anns = anns.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower()));
            }
            totalRecords = anns.Count();
            return anns.OrderByDescending(item => item.InsertDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}