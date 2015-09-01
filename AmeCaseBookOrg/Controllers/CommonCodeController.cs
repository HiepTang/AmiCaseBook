using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;
using MvcJqGrid;
using AutoMapper;
using AmeCaseBookOrg.Common;

namespace AmeCaseBookOrg.Controllers
{
    public class CommonCodeController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CommonCodeController( ICategoryService service)
        {
            _categoryService = service;
        }

        // GET: Country
        public ActionResult Index()
        {
            var mainMenus = _categoryService.GetMainCategories(false);
            return View(mainMenus);
        }

        // GET: Country/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = _categoryService.GetCountries().Where(c => c.Code == id) as Country;
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // GET: Country/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryId,CountryName,SortOrder")] Country country)
        {
            if (ModelState.IsValid)
            {
                
                return RedirectToAction("Index");
            }

            return View(country);
        }

        // GET: Country/Edit/5
        public ActionResult EditMainCode(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainCategory country = _categoryService.GetCategory(id.Value) as MainCategory;
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
        public ActionResult EditMainCode(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MainCategory mainCategory = _categoryService.GetCategory(viewModel.Code) as MainCategory;
                mainCategory = Mapper.Map<CategoryViewModel, MainCategory>(viewModel, mainCategory);
                _categoryService.SaveCategory();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        public ActionResult EditSubCode(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory country = _categoryService.GetCategory(id.Value) as SubCategory;
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
        public ActionResult EditSubCode(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                SubCategory subCategory = _categoryService.GetCategory(viewModel.Code) as SubCategory;
                subCategory = Mapper.Map<CategoryViewModel, SubCategory>(viewModel, subCategory);
                _categoryService.SaveCategory();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        // POST: Country/Delete/5
        [HttpPost]
        public JsonResult Delete(int? id)
        {
            if (id != null)
            {
                Category category = _categoryService.GetCategory(id.Value);
                if (category != null)
                {
                    try {
                        bool result = _categoryService.DeleteCategory(category);
                        if (!result)
                        {
                            return Json(new { result = false, message = "Cannot delete '" + category.CodeName + "'" });
                        }
                        _categoryService.SaveCategory();
                        return Json(new { result = true });
                    }
                    catch
                    {
                        return Json(new { result = false, message = "Cannot delete '" + category.CodeName + "'" });
                    }
                }
            }
            return Json(new { result = false, message = "Cannot delete this item" });
        }

        public JsonResult SearchMainCode(GridSettings gridSettings)
        {
            var mainMenus = _categoryService.GetMainCategories(false);
            if (mainMenus == null)
            {
                mainMenus = new List<MainCategory>();
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
        public JsonResult SearchSubCode(int mainCode, GridSettings gridSettings)
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
        public ActionResult CreateMainCode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMainCode(MainCategory model)
        {
            if (ModelState.IsValid)
            {
                model.IsMenu = false;
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
        public ActionResult CreateSubCode(int mainCode)
        {
            SubCategory subMenu = new SubCategory();
            subMenu.ParentCategoryCode = mainCode;
            return View(subMenu);
        }
        [HttpPost]
        public ActionResult CreateSubCode(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                subCategory.IsMenu = false;
                try
                {
                    if (_categoryService.GetCategory(subCategory.Code) == null)
                    {
                        _categoryService.CreateCategory(subCategory);
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
            return View(subCategory);
        }
        protected override void Dispose(bool disposing)
        {
           
            base.Dispose(disposing);
        }
    }
}
