using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.DAL.Infrastructure;

namespace AmeCaseBookOrg.Service
{
    public class CommunityService : ICommunityService
    {
        private ApplicationDbContext context;

        public CommunityService(IDbFactory dbFactory)
        {
            this.context = dbFactory.Init();
        }

        public void CreateComment(CommuityTopicComment comment)
        {
            this.context.CommuityTopicComments.Add(comment);
        }

        public void CreateTopic(CommunityTopic topic)
        {
            this.context.CommunityTopics.Add(topic);
        }

        public CommunityTopic GetTopic(int ID)
        {
            return this.context.CommunityTopics.Find(ID);
        }

        public IEnumerable<CommunityTopic> GetTopics()
        {
            return this.context.CommunityTopics;
        }

        public void SaveTopic()
        {
            this.context.SaveChanges();
        }

        public IEnumerable<CommunityTopic> Search(CommunityTopicSearchFilter filter, string sortColumn, string sortOrder, int pageSize, int pageIndex, out int totalRecords)
        {
            IEnumerable<CommunityTopic> topics = context.CommunityTopics;
            

            if (!string.IsNullOrEmpty(filter.Title))
            {
                topics = topics.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower()));
            }
            totalRecords = topics.Count();
            return topics.OrderByDescending(item => item.InsertDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public CommuityTopicComment GetComment(int id)
        {
            return this.context.CommuityTopicComments.FirstOrDefault(m => m.ID == id);
        }

        public void DeleteComment(CommuityTopicComment comment)
        {
            this.context.CommuityTopicComments.Remove(comment);
        }
    }
}