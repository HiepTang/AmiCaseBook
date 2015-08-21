using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;

namespace AmeCaseBookOrg.Controllers
{
    public class DataItemController : Controller
    {
        private readonly IDataItemService dataItemService;
        private readonly ICategoryService categoryService;

        public DataItemController(IDataItemService dataItemService, ICategoryService categoryService)
        {
            this.dataItemService = dataItemService;
            this.categoryService = categoryService;
        }
        
        // GET: DataItem
        public ActionResult Index()
        {
            return View();
        }

        [Route("DataItem/List/{code}/{countryCode}")]
        public ActionResult List(int code, int? countryCode)
        {
            if (code == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get the DataItems belong to this code
            IEnumerable<DataItem> dataItems;
            if(countryCode == 0) { 
                dataItems = dataItemService.GetDataItemsByMainCategory(code);
            }
            else
            {
                dataItems = dataItemService.GetDataItemsByCountry(code, countryCode.Value);
            }
                
            if(dataItems == null)
            {
                return HttpNotFound();
            }

            // Get country list
            var countries = categoryService.GetCountries();
            ViewBag.Countries = countries;

            // Get menu for this code
            MainMenu menu = (MainMenu)categoryService.GetCategory(code);
            ViewBag.Menu = menu;

            return View(dataItems);
        }
    }
}