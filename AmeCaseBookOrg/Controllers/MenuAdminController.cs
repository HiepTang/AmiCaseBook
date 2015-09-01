using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;
using Microsoft.AspNet.Identity.Owin;
using MvcJqGrid;
using AmeCaseBookOrg.Common;
using System.Net;
using AutoMapper;

namespace AmeCaseBookOrg.Controllers
{
    [Authorize(Roles = "Admin")]
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
            if(mainMenu == null)
            {
                return Json(new { records = 0 });
            }
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
        public ActionResult CreateMainMenu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMainMenu( MainMenu model)
        {
            if (ModelState.IsValid)
            {
                model.IsMenu = true;
                try
                {
                    if (_categoryService.GetCategory(model.Code) == null)
                    {
                        MainCategory mainCategory = _categoryService.GetMainCategories().Where(m => m.Code == (int)MainCategoryType.Menu).First();
                        model.ParentCategoryCode = mainCategory.Code;
                        _categoryService.CreateCategory(model);
                        _categoryService.SaveCategory();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Code", ErrorMessages.CATEGORYCODE_EXIST);
                    }

                }
                catch(Exception e)
                {
                    ModelState.AddModelError("", e);
                }
                           
                
            }
            return View(model);
        }
        public ActionResult CreateSubMenu(int mainMenuCode)
        {
            SubMenu subMenu = new SubMenu();
            subMenu.ParentCategoryCode = mainMenuCode;
            return View(subMenu);
        }
        [HttpPost]
        public ActionResult CreateSubMenu(SubMenu model)
        {
            if (ModelState.IsValid)
            {
                model.IsMenu = true;
                try
                {
                    if (_categoryService.GetCategory(model.Code) == null)
                    {
                        _categoryService.CreateCategory(model);
                        _categoryService.SaveCategory();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Code", ErrorMessages.CATEGORYCODE_EXIST);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e);
                }


            }
            return View(model);
        }
        public ActionResult EditMainMenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainMenu country = _categoryService.GetCategory(id.Value) as MainMenu;
            if (country == null)
            {
                return HttpNotFound();
            }
            CategoryViewModel model = Mapper.Map<CategoryViewModel>(country);
            return View(model);
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMainMenu(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MainMenu mainCategory = _categoryService.GetCategory(viewModel.Code) as MainMenu;
                mainCategory = Mapper.Map<CategoryViewModel, MainMenu>(viewModel, mainCategory);
                _categoryService.SaveCategory();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        public ActionResult EditSubMenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubMenu country = _categoryService.GetCategory(id.Value) as SubMenu;
            if (country == null)
            {
                return HttpNotFound();
            }
            CategoryViewModel model = Mapper.Map<CategoryViewModel>(country);
            return View(model);
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubMenu(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                SubMenu mainCategory = _categoryService.GetCategory(viewModel.Code) as SubMenu;
                mainCategory = Mapper.Map<CategoryViewModel, SubMenu>(viewModel, mainCategory);
                _categoryService.SaveCategory();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
    }
}