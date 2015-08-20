using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Service;
using AmeCaseBookOrg.Models;

namespace AmeCaseBookOrg.Controllers
{
    public class MenuController : Controller
    {
        private readonly ICategoryService categoryService;

        public MenuController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET: Menu
        public PartialViewResult MainMenu()
        {
            var mainMenus = categoryService.GetMainMenus();
            return PartialView(mainMenus);
        }
    }
}