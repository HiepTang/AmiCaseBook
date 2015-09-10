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
using System.Web.Routing;

namespace AmeCaseBookOrg.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ICategoryService categoryService;
        private readonly IFileService _fileService;
        private readonly IMemberService _memberService;
        public MemberController()
        {

        }

        public MemberController(ICategoryService categoryService, IFileService fileService, IMemberService memberService)
        {
            _fileService = fileService;
            this.categoryService = categoryService;
            this._memberService = memberService;
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
        public ActionResult Index(bool? ReloadData)
        {
            ViewBag.ReloadData = ReloadData;
            var applicationUsers = UserManager.Users.Include(a => a.Country).Include(a => a.UploadImage);
            return View(applicationUsers.ToList());
        }

        public JsonResult Search(GridSettings gridSettings)
        {
            MemberSearchFilter filter = new MemberSearchFilter();
            if (gridSettings.IsSearch)
            {
                filter.Email = gridSettings.Where.rules.Any(r => r.field == "Email") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "Email").data : string.Empty;
                filter.UserName = gridSettings.Where.rules.Any(r => r.field == "FullName") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "FullName").data : string.Empty;
            }
            int totalRecords = 0;
            var applicationUsers = _memberService.searchMember(filter, gridSettings.SortColumn, gridSettings.SortOrder, gridSettings.PageSize, gridSettings.PageIndex, out totalRecords);
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
                        FullName = a.LastName + ", " + a.FirstName,
                        Email = a.Email,
                        PhoneNumber = a.PhoneNumber,
                        Address = a.Address,
                        Country = a.Country.CodeName
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
            ViewBag.CountryId = new SelectList(categoryService.GetCountries(), "Code", "CodeName");           
            return View(new UserCreateViewModel());
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateViewModel model, int[] uploadedfile)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = Mapper.Map<ApplicationUser>(model);
                if (uploadedfile != null && uploadedfile.Length > 0)
                {
                    File newFile = _fileService.getFile(uploadedfile[uploadedfile.Length - 1]);   
                    if(newFile!=null)
                    {
                        applicationUser.FileId = newFile.FileId;
                    }
                }  
                    
                var result = UserManager.Create(applicationUser, model.Password);
                if (result.Succeeded)
                {
                    if (model.IsAdmin)
                    {
                        result = UserManager.AddToRole(applicationUser.Id, MemberRoles.Admin.ToString());
                    }
                    else
                    {
                        result = UserManager.AddToRole(applicationUser.Id, MemberRoles.Contributor.ToString());
                    }                
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index",  new  { ReloadData = true  });
                    }
                    
                }
                else
                {
                    AddErrors(result);
                }
            }
            if (uploadedfile != null)
            {
                    File file = _fileService.getFile(uploadedfile[uploadedfile.Length-1]);
                    if (file != null)
                        model.UploadImage=file;           
            }
            ViewBag.CountryId = new SelectList(categoryService.GetCountries(), "Code", "CodeName");
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
            ViewBag.CountryId = new SelectList(categoryService.GetCountries(), "Code", "CodeName", applicationUser.CountryId);
            if(applicationUser.FileId != null)
            {
                ViewBag.FileName = _fileService.getFile(applicationUser.FileId.Value).FileName;
            }
            
            UserViewModel model = Mapper.Map<UserViewModel>(applicationUser);
            var adminRole = _memberService.GetUserRoles().SingleOrDefault(r => r.Name == MemberRoles.Admin.ToString());
            model.IsAdmin = applicationUser.Roles.Any(r => r.RoleId == adminRole.Id);
            return View(model);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel model, int[] uploadedfile)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = UserManager.FindByName(model.Email);
                applicationUser = Mapper.Map<UserViewModel, ApplicationUser>(model, applicationUser);

                if (uploadedfile != null && uploadedfile.Length > 0)
                {
                    File newFile = _fileService.getFile(uploadedfile[uploadedfile.Length - 1]);
                     if (newFile != null)
                    {
                        if (applicationUser.UploadImage != null)
                        {
                            File oldFile = _fileService.getFile(applicationUser.UploadImage.FileId);
                            oldFile.FileName = newFile.FileName;
                            oldFile.Content = newFile.Content;
                            oldFile.ContentType = newFile.ContentType;
                            _fileService.saveFile();
                            if(newFile.FileId != oldFile.FileId)
                            {
                                _fileService.deleteFile(newFile);
                            }
                           
                        }
                        else
                        {
                            applicationUser.FileId = newFile.FileId;
                        }
                    } 
                }
                applicationUser.Roles.Clear();
                var result = UserManager.Update(applicationUser);
                if (result.Succeeded)
                {
                    if (model.IsAdmin)
                    {
                        result = UserManager.AddToRole(applicationUser.Id, MemberRoles.Admin.ToString());
                    }
                    else
                    {
                        result = UserManager.AddToRole(applicationUser.Id, MemberRoles.Contributor.ToString());
                    }
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", new { ReloadData = true} );
                    }

                }
                else
                {
                    AddErrors(result);
                }
                if (applicationUser.FileId != null)
                {
                    ViewBag.FileName = _fileService.getFile(applicationUser.FileId.Value).FileName;
                }
            }        
            ViewBag.CountryId = new SelectList(categoryService.GetCountries(), "Code", "CodeName", model.CountryId);         
            return View(model);
        }

        // GET: Member/Delete/5
        [HttpPost]
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
            IdentityResult result = UserManager.Delete(applicationUser);
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
