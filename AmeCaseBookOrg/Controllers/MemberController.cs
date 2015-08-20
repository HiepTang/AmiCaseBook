using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using MvcJqGrid;
using AmeCaseBookOrg.Service;
using AutoMapper;

namespace AmeCaseBookOrg.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ICategoryService categoryService;
        private readonly IFileService _fileService;

        public MemberController()
        {

        }

        public MemberController(ICategoryService categoryService, IFileService fileService)
        {
            _fileService = fileService;
            this.categoryService = categoryService;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Member
        public ActionResult Index()
        {
            
            var applicationUsers = UserManager.Users.Include(a => a.Country).Include(a => a.UploadImage);
            return View(applicationUsers.ToList());
        }

        public JsonResult Search(GridSettings gridSettings)
        {
            var applicationUsers = UserManager.Users;
            int totalRecords = applicationUsers.Count();
            var jsonData = new
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = (
                    from a in applicationUsers
                    select new
                    {
                        Id = a.Id,
                        FullName = a.FirstName,
                        Email = a.Email,
                        PhoneNumber = a.PhoneNumber
                    }
                )
            };
            return Json(jsonData);
        }

        // GET: Member/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Member/Create
        public ActionResult Create()
        {
            //ViewBag.Role = new SelectList(UserManager.get)
            ViewBag.CountryId = new SelectList(categoryService.GetCountries(), "Code", "CodeName");           
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel model, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = Mapper.Map<ApplicationUser>(model);
                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        var avatar = new File
                        {
                            FileName = upload.FileName,
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType,
                            Content = reader.ReadBytes(upload.ContentLength)
                        };
                        File outFile = _fileService.addFile(avatar);
                        applicationUser.UploadImage = outFile;
                    };


                }             
                var result = UserManager.Create(applicationUser, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }  
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        // GET: Member/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LastName,FirstName,Affiliation,Introduction,LinkIn,FileId,CountryId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            //ViewBag.CountryId = new SelectList(db.Categories, "Code", "CodeName", applicationUser.CountryId);
           //viewBag.FileId = new SelectList(db.Files, "FileId", "FileName", applicationUser.FileId);
            return View(applicationUser);
        }

        // GET: Member/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = UserManager.FindById(id);
            UserManager.Delete(applicationUser);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {           
            base.Dispose(disposing);
        }
    }
}
