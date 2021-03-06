﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;
using MvcJqGrid;
using System.Net;

namespace AmeCaseBookOrg.Controllers
{
    public class DataManagementController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IDataItemService dataItemService;
        private readonly IMemberService memberService;
        private readonly IFileService fileService;
        
        public DataManagementController(ICategoryService categoryService, IDataItemService dataItemService, IMemberService memberService, IFileService fileService)
        {
            this.categoryService = categoryService;
            this.dataItemService = dataItemService;
            this.memberService = memberService;
            this.fileService = fileService;
        }

        public ActionResult View(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dataItem = dataItemService.GetDataItem(id);

            if (dataItem == null)
            {
                return HttpNotFound();
            }

            var relatedItems = dataItemService.GetRelatedDataItems(dataItem).ToList();
            ViewBag.RelatedItems = relatedItems;
            if (TempData["fromMenu"] != null)
            {
                ViewBag.fromMenu = TempData["fromMenu"];
                TempData.Keep("fromMenu");
            }
            return View(dataItem);
        }

        [HttpPost]
        public JsonResult Comment(int topic, string comment, string email, string name)
        {
            if (topic == 0)
            {
                return Json(new { status = HttpStatusCode.BadRequest});
            }


            var cTopic = dataItemService.GetDataItem(topic);
            if (cTopic == null)
            {
                return Json(new { status = HttpStatusCode.NotFound });
            }

            if (String.IsNullOrEmpty(comment) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(name))
            {
                return Json(new { status = HttpStatusCode.NoContent });
            }

            Comment commentTopic = new Comment
            {
                Content = comment,
                Name = name,
                Email = email,
                ComemmentTime = DateTime.Now,
                DataItemID = topic
            };

            dataItemService.CreateComment(commentTopic);
            dataItemService.SaveDataItem();
            var jsonData = new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    Id = commentTopic.ID,
                    Content = commentTopic.Content,
                    Name = commentTopic.Name,
                    Email = commentTopic.Email,
                    ComemmentTime = commentTopic.ComemmentTime.ToString(),
                    DataItemID = commentTopic.DataItemID
                }
            };
            return Json(jsonData);
        }
        [HttpPost]
        [Authorize( Roles = "Admin")]
        public JsonResult DeleteComment(int id)
        {
            Comment comment = dataItemService.GetComment(id);
            if (comment != null)
            {
                dataItemService.DeleteComment(comment);
                dataItemService.SaveDataItem();
            }
            return Json(new { status = HttpStatusCode.OK });
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
            if(TempData["ReloadData"] != null)
            {
                ViewBag.ReloadData = TempData["ReloadData"];
                TempData["ReloadData"] = null;
            }
            TempData["fromMenu"] = null;
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

        [Authorize]
        // GET: DataManagement
        public ActionResult Create(String ReturnedURL)
        {
            var adminRole = memberService.GetUserRoles().SingleOrDefault(r => r.Name == MemberRoles.Admin.ToString());
            ApplicationUser currUser = memberService.GetUser(User.Identity.Name);
            // get submenu list
           
            IEnumerable<SubMenu> subMenus = null;
            if (currUser.Roles.Any(r =>r.RoleId == adminRole.Id))
            {
                ViewBag.CountryId = new SelectList(categoryService.GetCountries(), "Code", "CodeName");
                subMenus = categoryService.GetDataSubMenus().ToList();
            }
            else
            {
                List<SubCategory> country = new List<SubCategory>();
                country.Add(currUser.Country);
                ViewBag.CountryId = new SelectList(country, "Code", "CodeName");
                subMenus = categoryService.GetSubMenus(currUser) as IEnumerable<SubMenu>;
            }
            
            List<SubMenu> subMenuFullNames = new List<SubMenu>();
            foreach (var subMenu in subMenus)
            {
                string subMenuName = subMenu.GetMainMenu().CodeName + " > " + subMenu.CodeName;
                SubMenu viewModel = new SubMenu();
                viewModel.Code = subMenu.Code;
                viewModel.CodeName = subMenuName;
                subMenuFullNames.Add(viewModel);
            }
            ViewBag.SubCategoryID = new SelectList(subMenuFullNames,"Code","CodeName");
            ViewBag.ReturnedURL = ReturnedURL;
            return View(new DataItemViewModel());
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create(DataItemViewModel viewModel, int[] attachFileIds, int[]mainImageIds)
        {
            if (ModelState.IsValid)
            {
                DataItem model = AutoMapper.Mapper.Map<DataItem>(viewModel);
                if (attachFileIds != null)
                {
                    model.AttachFiles = new List<File>();
                    foreach (int id in attachFileIds)
                    {
                        File file = fileService.getFile(id);
                        if (file != null)
                        {
                            model.AttachFiles.Add(file);
                        }
                    }
                }
                if(mainImageIds != null && mainImageIds.Length > 0)
                {
                    model.Images = new List<File>();
                    File file = fileService.getFile(mainImageIds[mainImageIds.Length-1]);
                    if(file != null)
                    {
                        model.Images.Add(file);
                    }
                }
                model.LastUpdatedDate=DateTime.UtcNow;
                var user = memberService.GetUser(User.Identity.Name);
                model.LastUpdatedUserID = user.Id;
                model.CreatedDate = model.LastUpdatedDate;
                model.CreatedUserID = user.Id;
                dataItemService.CreateDataItem(model);
                SubMenu subMenu = categoryService.GetCategory(model.SubCategoryID) as SubMenu;
                if (subMenu != null)
                {
                    model.MainMenuID = subMenu.GetMainMenu().Code;
                }
                
                dataItemService.SaveDataItem();
                if (!string.IsNullOrEmpty(viewModel.ReturnedURL))
                {
                    string[] urls = viewModel.ReturnedURL.Split('/');
                    return RedirectToAction(urls[1], urls[0]);
                }
                TempData["ReloadData"] = true;
                return RedirectToAction("Index");
            }
           
            // get submenu list
            var adminRole = memberService.GetUserRoles().SingleOrDefault(r => r.Name == MemberRoles.Admin.ToString());
            ApplicationUser currUser = memberService.GetUser(User.Identity.Name);
            IEnumerable<SubMenu> subMenus = null;
            if (currUser.Roles.Any(r => r.RoleId == adminRole.Id))
            {
                subMenus = categoryService.GetDataSubMenus().ToList();
                ViewBag.CountryId = new SelectList(categoryService.GetCountries(), "Code", "CodeName");
            }
            else
            {
                List<SubCategory> country = new List<SubCategory>();
                country.Add(currUser.Country);
                ViewBag.CountryId = new SelectList(country, "Code", "CodeName");
                subMenus = categoryService.GetSubMenus(currUser) as IEnumerable<SubMenu>;
            }
            List<SubMenu> subMenuFullNames = new List<SubMenu>();
            foreach (var subMenu in subMenus)
            {
                string subMenuName = subMenu.GetMainMenu().CodeName + " > " + subMenu.CodeName;
                SubMenu SubViewModel = new SubMenu();
                SubViewModel.Code = subMenu.Code;
                SubViewModel.CodeName = subMenuName;
                subMenuFullNames.Add(SubViewModel);
            }
            ViewBag.SubCategoryID = new SelectList(subMenuFullNames, "Code", "CodeName");
            if (mainImageIds != null)
            {
                viewModel.Images = new List<File>();
                foreach (var id in mainImageIds)
                {
                    File file = fileService.getFile(id);
                    if (file != null)
                        viewModel.Images.Add(file);
                }
            }
            if (attachFileIds != null)
            {
                viewModel.AttachFiles = new List<File>();
                foreach (var id in attachFileIds)
                {
                    File file = fileService.getFile(id);
                    if (file != null)
                        viewModel.AttachFiles.Add(file);
                }
            }
            return View(viewModel);
        }
        [Authorize]
        // GET: DataManagement
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ann = dataItemService.GetDataItem(id);


            if (ann == null)
            {
                return HttpNotFound();
            }
            var adminRole = memberService.GetUserRoles().SingleOrDefault(r => r.Name == MemberRoles.Admin.ToString());
            ApplicationUser currUser = memberService.GetUser(User.Identity.Name);
            // Get Countries
           

            // get submenu list
            
            IEnumerable<SubMenu> subMenus = null;
            if (currUser.Roles.Any(r => r.RoleId == adminRole.Id))
            {
                ViewBag.CountryId = new SelectList(categoryService.GetCountries(), "Code", "CodeName", ann.CountryID);
                subMenus = categoryService.GetDataSubMenus().ToList();
            }
            else
            {
                List<SubCategory> country = new List<SubCategory>();
                country.Add(currUser.Country);
                ViewBag.CountryId = new SelectList(country, "Code", "CodeName", ann.CountryID);
                subMenus = categoryService.GetSubMenus(currUser) as IEnumerable<SubMenu>;
            }
            List<SubMenu> subMenuFullNames = new List<SubMenu>();
            foreach (var subMenu in subMenus)
            {
                string subMenuName = subMenu.GetMainMenu().CodeName + " > " + subMenu.CodeName;
                SubMenu SubMenuViewModel = new SubMenu();
                SubMenuViewModel.Code = subMenu.Code;
                SubMenuViewModel.CodeName = subMenuName;
                subMenuFullNames.Add(SubMenuViewModel);
            }
            ViewBag.SubCategoryID = new SelectList(subMenuFullNames, "Code", "CodeName", ann.SubCategoryID);
            DataItemViewModel viewModel = AutoMapper.Mapper.Map<DataItemViewModel>(ann);
            if (TempData["fromMenu"] != null)
            {
                viewModel.fromMenu = TempData["fromMenu"] as bool?;
                TempData.Keep("fromMenu");
            }
            return View(viewModel);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Edit(DataItemViewModel viewModel, int[] attachFileIds, int[] mainImageIds)
        {
            if (ModelState.IsValid)
            {
                DataItem model = dataItemService.GetDataItem(viewModel.ID);
                model = AutoMapper.Mapper.Map<DataItemViewModel, DataItem>(viewModel, model);
                if (mainImageIds != null && mainImageIds.Length > 0)
                {
                    foreach( int fileID in mainImageIds)
                    {
                        File file = fileService.getFile(fileID);
                        if (file != null)
                        {
                            if (model.Images == null)
                            {
                                model.Images = new List<File>();
                            }
                            if(model.Images.Count(i => i.FileId == fileID) == 0)
                            {
                                model.Images.Add(file);
                            }                            
                        }
                    }                                 
                }
                if (attachFileIds != null && attachFileIds.Length > 0)
                {
                    foreach (int id in attachFileIds)
                    {
                        File file = fileService.getFile(id);
                        if (file != null)
                        {
                            model.AttachFiles.Add(file);
                        }
                    }
                }
                model.LastUpdatedDate = DateTime.UtcNow;
                var user = memberService.GetUser(User.Identity.Name);
                model.LastUpdatedUserID = user.Id;
                SubMenu subMenu = categoryService.GetCategory(model.SubCategoryID) as SubMenu;
                if (subMenu != null)
                {
                    model.MainMenuID = subMenu.GetMainMenu().Code;
                }
                dataItemService.SaveDataItem();
                if(viewModel.fromMenu !=null && viewModel.fromMenu == true)
                {
                    return RedirectToAction("View", new { id = model.ID , fromMenu = true });
                }
                else
                {
                    TempData["ReloadData"] = true;
                    return RedirectToAction("View", new { id = model.ID});
                }
                
            }
            // get submenu list
            var adminRole = memberService.GetUserRoles().SingleOrDefault(r => r.Name == MemberRoles.Admin.ToString());
            ApplicationUser currUser = memberService.GetUser(User.Identity.Name);
            IEnumerable<SubMenu> subMenus = null;
            if (currUser.Roles.Any(r => r.RoleId == adminRole.Id))
            {
                ViewBag.CountryId = new SelectList(categoryService.GetCountries(), "Code", "CodeName", viewModel.CountryID);
                subMenus = categoryService.GetDataSubMenus().ToList();
            }
            else
            {
                List<SubCategory> country = new List<SubCategory>();
                country.Add(currUser.Country);
                ViewBag.CountryId = new SelectList(country, "Code", "CodeName");
                subMenus = categoryService.GetSubMenus(currUser) as IEnumerable<SubMenu>;
            }
            List<SubMenu> subMenuFullNames = new List<SubMenu>();
            foreach (var subMenu in subMenus)
            {
                string subMenuName = subMenu.GetMainMenu().CodeName + " > " + subMenu.CodeName;
                SubMenu SubMenuViewModel = new SubMenu();
                SubMenuViewModel.Code = subMenu.Code;
                SubMenuViewModel.CodeName = subMenuName;
                subMenuFullNames.Add(SubMenuViewModel);
            }
            ViewBag.SubCategoryID = new SelectList(subMenuFullNames, "Code", "CodeName", viewModel.SubCategoryID);
            if (mainImageIds != null)
            {
                if (viewModel.Images == null)
                    viewModel.Images = new List<File>();
                foreach (var id in mainImageIds)
                {
                    File file = fileService.getFile(id);
                    if (file != null)
                        viewModel.Images.Add(file);
                }
            }
            if (attachFileIds != null)
            {
                if(viewModel.AttachFiles == null)
                    viewModel.AttachFiles = new List<File>();
                foreach (var id in attachFileIds)
                {
                    File file = fileService.getFile(id);
                    if (file != null)
                        viewModel.AttachFiles.Add(file);
                }
            }
            viewModel.MainMenu = categoryService.GetCategory(viewModel.MainMenuID) as MainMenu;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize]
        public JsonResult Delete(int id)
        {
            DataItem item = dataItemService.GetDataItem(id);
            if(item == null)
            {
                return Json(new { status = HttpStatusCode.NoContent });
            }
            var adminRole = memberService.GetUserRoles().SingleOrDefault(r => r.Name == MemberRoles.Admin.ToString());
            ApplicationUser currUser = memberService.GetUser(User.Identity.Name);
            if (item.CreatedUser.UserName == User.Identity.Name || currUser.Roles.Any(r => r.RoleId == adminRole.Id))
            {
                dataItemService.DeleteItem(item);
                dataItemService.SaveDataItem();
                TempData["ReloadData"] = true;
                return Json(new { status = HttpStatusCode.OK });
            }
            return Json("");
        }
    }
}