using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;

namespace AmeCaseBookOrg.Controllers
{
    public class AboutController : Controller
    {
        private readonly IMemberService memberService;
        private readonly ICategoryService categoryService;

        public AboutController(IMemberService memberService, ICategoryService categoryService)
        {
            this.memberService = memberService;
            this.categoryService = categoryService;
        }
        



        [Route("About/Index/{code}/{countryCode}")]
        public ActionResult Index(int? code, int? countryCode)
        {
            return View();
        }

        public ActionResult Casebook()
        {
            return View();
        }

        public ActionResult Isgan()
        {
            return View();
            
        }

        public ActionResult Contributors()
        {
            var countries = categoryService.GetCountries();
            ViewBag.Countries = countries;

            var contributors = memberService.GetUserInRole(MemberRoles.Contributor.ToString());
            return View(contributors);
        }
    }
}