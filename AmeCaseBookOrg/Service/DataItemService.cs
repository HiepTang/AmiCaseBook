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
    }
}