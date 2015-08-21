using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmeCaseBookOrg.Models;

namespace AmeCaseBookOrg.Service
{
    public interface IDataItemService
    {
        IEnumerable<DataItem> GetDataItems();

        IEnumerable<DataItem> GetDataItemsByMainCategory(int mainMenuID);

        IEnumerable<DataItem> GetDataItemsByCountry(int mainMenuID, int countryCode);

        IEnumerable<DataItem> GetDataItemsByCountry(int countryCode);

        DataItem GetDataItem(int ID);

        void CreateDataItem(DataItem dataItem);

        void SaveDataItem();


    }
}
