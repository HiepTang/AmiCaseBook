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
        public ActionResult Index()
        {
            var mainMenus = _categoryService.GetMainMenus();
            return View(mainMenus);
        }
        public JsonResult SearchMainMenu(GridSettings gridSettings)
        {          
            var mainMenus = _categoryService.GetMainMenus();         
            if(mainMenus == null)
            {
                mainMenus = new List<MainMenu>();
            }
            int totalRecords = mainMenus.Count();
            var jsonData = new
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = (
                   from a in mainMenus
                   select new
                   {
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
            var jsonData = new
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = (
                   from a in subMenus
                   select new
                   {
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