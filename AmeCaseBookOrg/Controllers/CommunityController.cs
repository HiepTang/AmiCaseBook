using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;
using MvcJqGrid;
using System.Net;

namespace AmeCaseBookOrg.Controllers
{
    public class CommunityController : Controller
    {

        private readonly ICommunityService communityService;
        private readonly IMemberService memberService;

        public CommunityController(ICommunityService communityService, IMemberService memberService)
        {
            this.communityService = communityService;
            this.memberService = memberService;
        }
        // GET: Community
        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult Search(GridSettings gridSettings)
        {
            CommunityTopicSearchFilter filter = new CommunityTopicSearchFilter();
            if (gridSettings.IsSearch)
            {
                filter.Title = gridSettings.Where.rules.Any(r => r.field == "Title") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "Title").data : string.Empty;
            }
            int totalRecords = 0;
            var topics = communityService.Search(filter, gridSettings.SortColumn, gridSettings.SortOrder, gridSettings.PageSize, gridSettings.PageIndex, out totalRecords);
            
            var jsonData = new
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = (
                    from a in topics
                    select new
                    {
                        ID = a.ID,
                        Title = a.Title,
                        InsertDate = a.InsertDate
                    }
                )
            };

            JsonResult result = Json(jsonData);
            return result;
        }

        public ActionResult View(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var topic = communityService.GetTopic(id);


            if (topic == null)
            {
                return HttpNotFound();
            }

            topic.Hit = topic.Hit + 1;
            communityService.SaveTopic(topic);

            return View(topic);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Comment(int topic, string comment)
        {
            if(topic == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var cTopic = communityService.GetTopic(topic);
            if(cTopic == null)
            {
                return HttpNotFound();
            }

            if (String.IsNullOrEmpty(comment))
            {
                return View("View", cTopic);
            }

            ApplicationUser user = memberService.GetUser(User.Identity.Name);
            

            CommuityTopicComment commentTopic = new CommuityTopicComment
            {
                Content = comment,
                ComemmentTime = DateTime.Now,
                CommentUserId = user.Id,
                CommunityTopicID = topic
            };

            communityService.CreateComment(commentTopic);
            communityService.SaveTopic(cTopic);

            return View("View", cTopic);
        }
    }
}