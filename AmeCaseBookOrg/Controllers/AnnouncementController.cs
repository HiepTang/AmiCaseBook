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
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Announcement model, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    String fileName = System.IO.Path.GetFileName(upload.FileName);
                    String contentType = upload.ContentType;
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        var avatar = new File
                        {
                            FileName = fileName,
                            FileType = FileType.Avatar,
                            ContentType = contentType,
                            Content = reader.ReadBytes(upload.ContentLength)
                        };
                        File outFile = fileService.addFile(avatar);
                        model.AttachmentFiles = new List<File>();
                        model.AttachmentFiles.Add(outFile);                   
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
            return View(model);
        }
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
        public ActionResult Edit(AnnouncementViewModel model, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                Announcement ann = announcementService.GetAnnouncement(model.ID);
                ann = AutoMapper.Mapper.Map<AnnouncementViewModel, Announcement>(model, ann);
                if (upload != null && upload.ContentLength > 0)
                {
                    String fileName = System.IO.Path.GetFileName(upload.FileName);
                    String contentType = upload.ContentType;
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        var avatar = new File
                        {
                            FileName = fileName,
                            FileType = FileType.Avatar,
                            ContentType = contentType,
                            Content = reader.ReadBytes(upload.ContentLength)
                        };
                        //TODO Update File here
                        
                    }
                }
                ann.LastUpdatedDate = DateTime.UtcNow;
                ApplicationUser user = memberService.GetUser(User.Identity.Name);
                ann.AuthorUserID = user.Id;
                ann.LastUpdatedUserID = user.Id;
                announcementService.SaveAnnouncement();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}