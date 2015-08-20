using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Service;
using AmeCaseBookOrg.Models;
using Microsoft.AspNet.Identity.Owin;

namespace AmeCaseBookOrg.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly IAnnouncementService announcementService;
        private readonly ICategoryService categoryService;
        private readonly IMemberService memberService;


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


        public HomeController(IAnnouncementService announcementService, ICategoryService categoryService, IMemberService memberService)
        {
            this.announcementService = announcementService;
            this.categoryService = categoryService;
            this.memberService = memberService;
        }
        public ActionResult Index()
        {
            var announcements = announcementService.GetAnnouncements();
            ViewBag.Announcements = announcements.OrderByDescending(item => item.InsertDate).Take(5);

            var countries = categoryService.GetCountries();
            ViewBag.Countries = countries;

            var users = memberService.GetUserInRole("Contributor");
            ViewBag.Contributors = users;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}