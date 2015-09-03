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

        IEnumerable<DataItem> GetRelatedDataItems(DataItem dataItem);

        IEnumerable<DataItem> GetDataItemsBySubMenu(int subMenuCode);

        IEnumerable<DataItem> GetDataItemsByMainCategory(int mainMenuID);

        IEnumerable<DataItem> GetDataItemsByCountry(int mainMenuID, int countryCode);

        IEnumerable<DataItem> GetDataItemsByCountry(int countryCode);

        IEnumerable<DataItem> Search(DataItemSearchFilter filter, string sortColumn, string sortOrder, int pageSize, int pageIndex, out int totalRecords);

        DataItem GetDataItem(int ID);

        void CreateDataItem(DataItem dataItem);

        void SaveDataItem();

        void CreateComment(Comment comment);

        void DeleteComment(Comment comment);

        Comment GetComment(int id);

        void DeleteItem(DataItem item);

    }
}
