using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;
using MvcJqGrid;

namespace AmeCaseBookOrg.Controllers
{
    public class DataManagementController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IDataItemService dataItemService;
        private readonly IMemberService memberService;
        
        public DataManagementController(ICategoryService categoryService, IDataItemService dataItemService, IMemberService memberService)
        {
            this.categoryService = categoryService;
            this.dataItemService = dataItemService;
            this.memberService = memberService;
        }

        // GET: DataManagement
        public ActionResult Index()
        {
            // Get Countries
            var countries = categoryService.GetCountries();
            ViewBag.Countries = (from c in countries select c.CodeName).ToArray();

            // Get authors
            var authors = memberService.GetUsers();
            ViewBag.Authors = (from a in authors select a.FullName).ToArray();

            // get submenu list
            var subMenus = categoryService.GetSubMenus().ToList();
            List<string> subMenuFullNames = new List<string>();
            foreach (var subMenu in subMenus)
            {
                string subMenuName = subMenu.GetMainMenu().CodeName + " > " + subMenu.CodeName;
                subMenuFullNames.Add(subMenuName);
            }
            ViewBag.SubMenus = subMenuFullNames.ToArray();

            return View();
        }

        public JsonResult Search(GridSettings gridSettings)
        {
            DataItemSearchFilter filter = new DataItemSearchFilter();
            if (gridSettings.IsSearch)
            {
                filter.Title = gridSettings.Where.rules.Any(r => r.field == "Title") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "Title").data : string.Empty;

                filter.AuthorName = gridSettings.Where.rules.Any(r => r.field == "Author") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "Author").data : string.Empty;

                filter.CountryName = gridSettings.Where.rules.Any(r => r.field == "Country") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "Country").data : string.Empty;

                filter.SubMenuName = gridSettings.Where.rules.Any(r => r.field == "SubMenuName") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "SubMenuName").data : string.Empty;
            }
            int totalRecords = 0;
            var items = dataItemService.Search(filter, gridSettings.SortColumn, gridSettings.SortOrder, gridSettings.PageSize, gridSettings.PageIndex, out totalRecords);

            var jsonData = new
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = (
                    from a in items
                    select new
                    {
                        ID = a.ID,
                        Title = a.Title,
                        SubMenuName = a.MainMenu.CodeName + " > "+ a.SubCategory.CodeName,
                        Country = a.Country.CodeName,
                        InsertDate = a.CreatedDate,
                        EditDate = a.LastUpdatedDate,
                        Author = a.CreatedUser.FullName
                    }
                )
            };

            JsonResult result = Json(jsonData);
            return result;
        }
    }
}