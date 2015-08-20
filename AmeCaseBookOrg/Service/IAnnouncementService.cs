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

        void CreateAnnouncement(Announcement announcement);

        void SaveAnnouncement();
    }
}
