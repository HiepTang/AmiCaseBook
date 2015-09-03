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
        private readonly IFileService fileService;

        public CommunityController(ICommunityService communityService, IMemberService memberService, IFileService fileService)
        {
            this.communityService = communityService;
            this.memberService = memberService;
            this.fileService = fileService;
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
            communityService.SaveTopic();

            return View(topic);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Comment(int topic, string comment)
        {
            if (topic == 0)
            {
                return Json(new { status = HttpStatusCode.BadRequest });
            }


            var cTopic = communityService.GetTopic(topic);
            if (cTopic == null)
            {
                return Json(new { status = HttpStatusCode.NotFound });
            }

            if (String.IsNullOrEmpty(comment))
            {
                return Json(new { status = HttpStatusCode.NoContent });
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
            communityService.SaveTopic();
            var jsonData = new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    Id = commentTopic.ID,
                    Content = commentTopic.Content,
                    Name = commentTopic.CommentUser.FullName,                   
                    ComemmentTime = commentTopic.ComemmentTime.ToString()
                }
            };
            return Json(jsonData);
        }
        [HttpPost]
        public JsonResult DeleteComment(int id)
        {
            CommuityTopicComment comment = this.communityService.GetComment(id);
            if (comment != null)
            {
                if(comment.CommentUser.UserName == User.Identity.Name)
                {
                    communityService.DeleteComment(comment);
                    communityService.SaveTopic();
                }
                else
                {
                    return Json("");
                }
            }
            return Json(new { status = HttpStatusCode.OK });
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CommunityTopic model, int[] upoadedfile)
        {
            if (ModelState.IsValid)
            {
                if (upoadedfile != null)
                {
                    model.AttachmentFiles = new List<File>();
                    foreach (int id in upoadedfile)
                    {
                        File file = fileService.getFile(id);
                        if (file != null)
                        {
                            model.AttachmentFiles.Add(file);
                        }
                    }
                }
                model.InsertDate = DateTime.UtcNow;
                model.LastUpdatedDate = DateTime.UtcNow;
                ApplicationUser user = memberService.GetUser(User.Identity.Name);
                model.AuthorUserID = user.Id;
                model.LastUpdatedUserID = user.Id;
                communityService.CreateTopic(model);
                communityService.SaveTopic();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ann = communityService.GetTopic(id);


            if (ann == null)
            {
                return HttpNotFound();
            }
            CommunityTopicViewModel viewModel = AutoMapper.Mapper.Map<CommunityTopicViewModel>(ann);

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Edit(CommunityTopicViewModel model, int[] upoadedfile)
        {
            if (ModelState.IsValid)
            {
                CommunityTopic ann = communityService.GetTopic(model.ID);
                ann = AutoMapper.Mapper.Map<CommunityTopicViewModel, CommunityTopic>(model, ann);
                if (upoadedfile != null && upoadedfile.Length > 0)
                {
                    foreach (int id in upoadedfile)
                    {
                        File file = fileService.getFile(id);
                        if (file != null)
                        {
                            ann.AttachmentFiles.Add(file);
                        }
                    }
                }
                ann.LastUpdatedDate = DateTime.UtcNow;
                ApplicationUser user = memberService.GetUser(User.Identity.Name);
                ann.AuthorUserID = user.Id;
                ann.LastUpdatedUserID = user.Id;
                communityService.SaveTopic();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            CommunityTopic item = this.communityService.GetTopic(id);
            if (item == null)
            {
                return Json(new { status = HttpStatusCode.NoContent });
            }
            var adminRole = memberService.GetUserRoles().SingleOrDefault(r => r.Name == MemberRoles.Admin.ToString());
            ApplicationUser currUser = memberService.GetUser(User.Identity.Name);
            if (item.AuthorUserID == currUser.Id || currUser.Roles.Any(r => r.RoleId == adminRole.Id))
            {
                communityService.DeleteTopic(item);
                communityService.SaveTopic();
                return Json(new { status = HttpStatusCode.OK });
            }
            return Json("");
        }
    }
}