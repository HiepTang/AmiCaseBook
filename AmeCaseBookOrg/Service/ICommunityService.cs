using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmeCaseBookOrg.Models;

namespace AmeCaseBookOrg.Service
{
    public interface ICommunityService
    {

        CommunityTopic GetTopic(int ID);

        IEnumerable<CommunityTopic> GetTopics();

        IEnumerable<CommunityTopic> Search(CommunityTopicSearchFilter filter, string sortColumn, string sortOrder, int pageSize, int pageIndex, out int totalRecords);

        void CreateTopic(CommunityTopic topic);

        void CreateComment(CommuityTopicComment comment);

        void SaveTopic(CommunityTopic topic);
    }
}
