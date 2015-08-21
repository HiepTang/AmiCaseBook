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
            if (code == 0 && countryCode == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get the DataItems belong to this code
            IEnumerable<DataItem> dataItems;
            if(countryCode == 0 && code != 0) { 
                dataItems = dataItemService.GetDataItemsByMainCategory(code);
            }
            else
            {
                if (countryCode != 0 && code == 0)
                {
                    dataItems = dataItemService.GetDataItemsByCountry(countryCode.Value);
                }
                else
                {
                    dataItems = dataItemService.GetDataItemsByCountry(code, countryCode.Value);
                }

                
            }
                
            if(dataItems == null)
            {
                return HttpNotFound();
            }

            // Get country list
            var countries = categoryService.GetCountries();
            ViewBag.Countries = countries;

            // Get menu for this code
            if (code != 0)
            {
                MainMenu menu = (MainMenu)categoryService.GetCategory(code);
                ViewBag.Menu = menu;
            }

            if(countryCode != 0)
            {
                var country = categoryService.GetCategory(countryCode.Value);
                ViewBag.Country = country;
            }
            return View(dataItems);
        }

        public ActionResult View(int id)
        {
            if(id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dataItem = dataItemService.GetDataItem(id);

            if(dataItem == null)
            {
                return HttpNotFound();
            }
            
            
            return View(dataItem);
        }
    }
}