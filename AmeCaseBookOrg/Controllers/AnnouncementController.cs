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
    public class AnnouncementController : Controller
    {

        private readonly IAnnouncementService announcementService;
        private readonly IMemberService memberService;
        private readonly IFileService fileService;

        public AnnouncementController(IAnnouncementService announcementService, IMemberService memberService, IFileService fileService)
        {
            this.announcementService = announcementService;
            this.memberService = memberService;
            this.fileService = fileService;
        }
        // GET: Announcement
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Search(GridSettings gridSettings)
        {
            AnnouncementSearchFilter filter = new AnnouncementSearchFilter();
            if (gridSettings.IsSearch)
            {
                filter.Title = gridSettings.Where.rules.Any(r => r.field == "Title") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "Title").data : string.Empty;
            }
            int totalRecords = 0;
            var anss = announcementService.Search(filter, gridSettings.SortColumn, gridSettings.SortOrder, gridSettings.PageSize, gridSettings.PageIndex, out totalRecords);

            var jsonData = new
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = (
                    from a in anss
                    select new
                    {
                        ID = a.ID,
                        Title = a.Title,
                        InsertDate = a.InsertDate,
                        EditDate = a.LastUpdatedDate,
                        Author = a.Author.FullName
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

            var ann = announcementService.GetAnnouncement(id);
            

            if (ann == null)
            {
                return HttpNotFound();
            }

        
            return View(ann);
        }
        [Authorize]
        public ActionResult Create()
        {
            return View(new Announcement());
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create(Announcement model, int[] uploadedfile)
        {
            if (ModelState.IsValid)
            {
                if(uploadedfile != null)
                {
                    model.AttachmentFiles = new List<File>();
                    foreach(int id in uploadedfile)
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
                announcementService.CreateAnnouncement(model);
                announcementService.SaveAnnouncement();
                return RedirectToAction("Index");
            }
            if (uploadedfile != null)
            {
                model.AttachmentFiles = new List<File>();
                foreach (var id in uploadedfile)
                {
                    File file = fileService.getFile(id);
                    if (file != null)
                        model.AttachmentFiles.Add(file);
                }
            }
            return View(model);
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ann = announcementService.GetAnnouncement(id);


            if (ann == null)
            {
                return HttpNotFound();
            }
            AnnouncementViewModel viewModel = AutoMapper.Mapper.Map<AnnouncementViewModel>(ann);

            return View(viewModel);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Edit(AnnouncementViewModel model, int[] uploadedfile)
        {
            if (ModelState.IsValid)
            {
                Announcement ann = announcementService.GetAnnouncement(model.ID);
                ann = AutoMapper.Mapper.Map<AnnouncementViewModel, Announcement>(model, ann);
                if (uploadedfile != null && uploadedfile.Length > 0)
                {
                    foreach( int id in uploadedfile)
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
                announcementService.SaveAnnouncement();
                return RedirectToAction("View", new { id = ann.ID});
            }
            if (uploadedfile != null)
            {
                if(model.AttachmentFiles == null)
                    model.AttachmentFiles = new List<File>();
                foreach (var id in uploadedfile)
                {
                    File file = fileService.getFile(id);
                    if (file != null)
                        model.AttachmentFiles.Add(file);
                }
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult Delete( int id)
        {
            Announcement item = this.announcementService.GetAnnouncement(id);
            if (item == null)
            {
                return Json(new { status = HttpStatusCode.NoContent });
            }
            var adminRole = memberService.GetUserRoles().SingleOrDefault(r => r.Name == MemberRoles.Admin.ToString());
            ApplicationUser currUser = memberService.GetUser(User.Identity.Name);
            if (item.AuthorUserID == currUser.Id || currUser.Roles.Any(r => r.RoleId == adminRole.Id))
            {
                announcementService.DeleteAnnouncement(item);
                announcementService.SaveAnnouncement();
                return Json(new { status = HttpStatusCode.OK });
            }
            return Json("");
        }
    }
}