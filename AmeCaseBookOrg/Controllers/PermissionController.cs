using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MvcJqGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AmeCaseBookOrg.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PermissionController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ICategoryService _categoryService;
        private readonly IMemberService _memberService;
        public PermissionController(ICategoryService service, IMemberService memberService)
        {
            _categoryService = service;
            _memberService = memberService;
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
        // GET: Permission
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetAllSubMenu(string userName)
        {
            //var userName = "phuc0903@gmail.com";
            ApplicationUser user = await UserManager.FindByEmailAsync(userName);
            IEnumerable<SubCategory> currentSubMenus = _categoryService.GetSubMenus(user);
            IEnumerable<MainMenu> mainMenus = _categoryService.GetMainMenus().ToList();
            List<SubCategory> subMenus = new List<SubCategory>();
            foreach ( var mainMenu in mainMenus)
            {
                if(mainMenu.GetSubMenus() != null)
                {
                    foreach (var subMenu in mainMenu.GetSubMenus())
                    {
                        subMenus.Add(subMenu);
                    }
                }
            }
           
            var jsonData = new
            {
                rows = 
                (
                    from m in subMenus
                    select new
                    {
                        Code = m.Code,
                        CodeName = m.ParentCategory.CodeName + " > " + m.CodeName,
                        IsSelected = (currentSubMenus.Count(a => a.Code == m.Code) > 0)
                    }
                )
            };

            return Json(jsonData,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> SavePermission(string UserName, ICollection<int> Codes) {
            if(Codes == null)
            {
                Codes = new List<int>();
            }
            try
            {
                ApplicationDbContext dbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                ApplicationUser user = await UserManager.FindByEmailAsync(UserName);
                IEnumerable<SubCategory> currentSubMenus = user.CanAccessCategories;
                IEnumerable<SubCategory> menuToDelete = from subMenu in currentSubMenus where !Codes.Contains(subMenu.Code) select subMenu;
                foreach (var m in menuToDelete.ToList())
                {
                    user.CanAccessCategories.Remove(m as SubMenu);
                }
                foreach (int code in Codes)
                {
                    if (currentSubMenus.Count(m => m.Code == code) == 0)
                    {
                        SubMenu menu = dbContext.Categories.Where(c => c.Code == code).First() as SubMenu;
                        user.CanAccessCategories.Add(menu);
                    }
                }
                int i = dbContext.SaveChanges();
                return Json(new {result = i>=0});
            }
            catch(Exception e)
            {
                return Json(new { result = false, message = e.Message });
            }
           
        }
        public JsonResult SearchMember(GridSettings gridSettings)
        {
            MemberSearchFilter filter = new MemberSearchFilter();
            if (gridSettings.IsSearch)
            {
                filter.Email = gridSettings.Where.rules.Any(r => r.field == "Email") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "Email").data : string.Empty;
                filter.UserName = gridSettings.Where.rules.Any(r => r.field == "FullName") ?
                        gridSettings.Where.rules.FirstOrDefault(r => r.field == "FullName").data : string.Empty;
            }
            filter.Role = MemberRoles.Contributor;
            int totalRecords = 0;
            var applicationUsers = _memberService.searchMember(filter, gridSettings.SortColumn, gridSettings.SortOrder, gridSettings.PageSize, gridSettings.PageIndex, out totalRecords);
            var jsonData = new
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = (
                    from a in applicationUsers
                    select new
                    {
                        Id = a.Id,
                        FullName = a.FullName,
                        Email = a.Email,
                        PhoneNumber = a.PhoneNumber,
                        Country = a.Country.CodeName
                    }
                )
            };
            return Json(jsonData);
        }
    }
   
}