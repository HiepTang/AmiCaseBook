using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.DAL.Infrastructure;

namespace AmeCaseBookOrg.Service
{
    public class DataItemService : IDataItemService
    {
        private ApplicationDbContext context;

        public DataItemService(IDbFactory dbFactory)
        {
            context = dbFactory.Init();
        }
        public void CreateDataItem(DataItem dataItem)
        {
            this.context.DataItems.Add(dataItem);
        }

        public DataItem GetDataItem(int ID)
        {
            return this.context.DataItems.Find(ID);
        }

        public IEnumerable<DataItem> GetDataItems()
        {
            return this.context.DataItems;
        }

        public IEnumerable<DataItem> GetDataItemsByCountry(int mainMenuID, int countryCode)
        {
            var dataItems = this.context.DataItems.Where(d => d.MainMenuID== mainMenuID && d.CountryID == countryCode);
            return dataItems;
        }

        public IEnumerable<DataItem> GetDataItemsByCountry(int countryCode)
        {
            var dataItems = this.context.DataItems.Where(d => d.CountryID == countryCode);
            return dataItems;
        }


        public IEnumerable<DataItem> GetDataItemsByMainCategory(int mainMenuID)
        {
            var dataItems = this.context.DataItems.Where(d => d.MainMenuID== mainMenuID);
            return dataItems;
        }

        public void SaveDataItem()
        {
            this.context.SaveChanges();
        }

        public IEnumerable<DataItem> Search(DataItemSearchFilter filter, string sortColumn, string sortOrder, int pageSize, int pageIndex, out int totalRecords)
        {
            IEnumerable<DataItem> items = context.DataItems.ToList();


            if (!string.IsNullOrEmpty(filter.Title))
            {
               items = items.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.CountryName))
            {
                var country = this.context.SubCategories.Where(c => c.ParentCategoryCode == (int)MainCategoryType.Country && c.CodeName.Contains(filter.CountryName.ToLower())).FirstOrDefault();
                if (country != null)
                {
                    items = items.Where(t => t.CountryID == country.Code);
                }
            }

            if (!string.IsNullOrEmpty(filter.SubMenuName))
            {
                string[] menus = filter.SubMenuName.Split('>');
                if(menus.Count() > 1)
                {
                    string mainMenu = menus[0].Trim();
                    string subMenu = menus[1].Trim();

                    var main = this.context.MainMenus.Where(m => m.CodeName.Contains(mainMenu.ToLower())).FirstOrDefault();
                    if(main != null)
                    {
                        var sub = this.context.SubMenus.Where(m => m.ParentCategoryCode == main.Code && m.CodeName.Contains(subMenu.ToLower())).FirstOrDefault();
                        if(sub != null)
                        {
                            items = items.Where(t => t.SubCategoryID == sub.Code);
                        }

                    }

                }

            }

            if (!string.IsNullOrEmpty(filter.AuthorName))
            {
                
                     items = items.Where(t => t.CreatedUser.FullName.ToLower().Contains(filter.AuthorName.ToLower()));
   
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                items = items.Where(t => t.CreatedUser.Email.ToLower().Contains(filter.Email.ToLower()));
            }
            totalRecords = items.Count();
            return items.OrderByDescending(item => item.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        }

        public IEnumerable<DataItem> GetDataItemsBySubMenu(int subMenuCode)
        {
            var dataItems = this.context.DataItems.Where(d => d.SubCategoryID == subMenuCode);
            return dataItems;
        }

        public IEnumerable<DataItem> GetRelatedDataItems(DataItem dataItem)
        {
            var dataItems = this.context.DataItems.Where(d => d.SubCategoryID == dataItem.SubCategoryID && d.ID != dataItem.ID);
            return dataItems.OrderByDescending(d => d.CreatedDate).Take(3);
        }

        public void CreateComment(Comment comment)
        {
            this.context.Comments.Add(comment);
        }
        
        public void DeleteComment(Comment comment)
        {
            this.context.Comments.Remove(comment);
        }

        public Comment GetComment(int id)
        {
            return this.context.Comments.FirstOrDefault(m => m.ID == id);
        }
    }
}