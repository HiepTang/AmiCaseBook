using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmeCaseBookOrg.Models;

namespace AmeCaseBookOrg.Service
{
    public interface IAnnouncementService
    {


        IEnumerable<Announcement> GetAnnouncements();

        IEnumerable<Announcement> Search(AnnouncementSearchFilter filter, string sortColumn, string sortOrder, int pageSize, int pageIndex, out int totalRecords);

        Announcement GetAnnouncement(int id);

        void CreateAnnouncement(Announcement announcement);

        void DeleteAnnouncement(Announcement announcement);

        void SaveAnnouncement();
    }
}
