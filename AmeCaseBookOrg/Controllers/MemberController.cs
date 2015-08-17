﻿using System;
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


namespace AmeCaseBookOrg.Controllers
{
    public class MemberController : Controller
    {
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();


        public MemberController()
        {

        }

        public MemberController(ApplicationUserManager userManager)
        {

            UserManager = userManager; 
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
            var applicationUsers = UserManager.Users.Include(a => a.Country).Include(a => a.UploadImage);
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
                        FullName = a.FullName,
                        Email = a.Email,
                        PhoneNumber = a.PhoneNumber,
                        Country = a.Country
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
            ViewBag.CountryId = new SelectList(db.Categories, "Code", "CodeName");
            ViewBag.FileId = new SelectList(db.Files, "FileId", "FileName");
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LastName,FirstName,Affiliation,Introduction,LinkIn,FileId,CountryId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                UserManager.Create(applicationUser, applicationUser.PasswordHash);
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Categories, "Code", "CodeName", applicationUser.CountryId);
            ViewBag.FileId = new SelectList(db.Files, "FileId", "FileName", applicationUser.FileId);
            return View(applicationUser);
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
            ViewBag.CountryId = new SelectList(db.Categories, "Code", "CodeName", applicationUser.CountryId);
            ViewBag.FileId = new SelectList(db.Files, "FileId", "FileName", applicationUser.FileId);
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
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Categories, "Code", "CodeName", applicationUser.CountryId);
            ViewBag.FileId = new SelectList(db.Files, "FileId", "FileName", applicationUser.FileId);
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
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}