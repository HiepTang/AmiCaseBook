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

        public CommunityController(ICommunityService communityService)
        {
            this.communityService = communityService;
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
            return View(topic);
        }

        [HttpPost]
        public ActionResult Comment(int topic, string comment)
        {
            if(topic == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }




            return View();
        }
    }
}