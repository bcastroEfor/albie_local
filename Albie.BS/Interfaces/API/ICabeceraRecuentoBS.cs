using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface ICabeceraRecuentoBS
    {
        ResultAndError<CabeceraRecuento> Add(CabeceraRecuento pr);
        CabeceraRecuento Get(int id);
        ResultAndError<CabeceraRecuento> Update(CabeceraRecuento pr, bool insertIfNoExists = false);
        CollectionList<CabeceraRecuento> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
    }
}