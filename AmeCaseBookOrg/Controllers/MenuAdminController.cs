using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;
using Microsoft.AspNet.Identity.Owin;
using MvcJqGrid;

namespace AmeCaseBookOrg.Controllers
{
    public class MenuAdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private ICategoryService _categoryService;
        public MenuAdminController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // GET: CommonCode
        public ActionResult Index(int? mainCode)
        {
            if (mainCode != null)
            {
                ViewBag.MainCode = mainCode;
            }
            return View();
        }
        public JsonResult SearchMainMenu(GridSettings gridSettings)
        {          
            var mainMenus = _categoryService.GetMainMenus();         
            if(mainMenus == null)
            {
                mainMenus = new List<MainMenu>();
            }
            int totalRecords = mainMenus.Count();
            int index = 1;
            var jsonData = new
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = (
                   from a in mainMenus
                   select new
                   {
                       No = index++,
                       Code = a.Code,
                       CodeName = a.CodeName,
                       URL = a.URL,
                       Memo = a.Memo
                   }
               )
            };
            return Json(jsonData);
        }
        public JsonResult SearchSubMenu(int mainCode, GridSettings gridSettings)
        {            
            var mainMenu = _categoryService.GetCategory(mainCode);
            var subMenus = mainMenu.SubCategories;
            var totalRecords = subMenus.Count();
            int index = 1;
            var jsonData = new
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = (
                   from a in subMenus
                   select new
                   {
                       No = index++,
                       Code = a.Code,
                       CodeName = a.CodeName,
                       URL = a.URL,
                       Memo = a.Memo
                   }
               )
            };
            return Json(jsonData);
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
    }
}