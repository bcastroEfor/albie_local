using System;
using System.Collections.Generic;
using System.Linq;
using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface IAlbaranLineaBS
    {
        ResultAndError<AlbaranLinea> Add(AlbaranLinea c);
        ResultAndError<bool> Delete(int id, string albaranNo);
        bool DeleteMulti(IEnumerable<KeyValuePair<string, int>> AlbaranLineas);
        AlbaranLinea Get(int id, string albaranNo);
        CollectionList<AlbaranLinea> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
        CollectionList<AlbaranLinea> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "");
        ResultAndError<AlbaranLinea> Update(AlbaranLinea cr, bool insertIfNoExists = false);
        bool UpdateMulti(IEnumerable<AlbaranLinea> oAlbaranLineas, bool insertIfNoExists = false);
        ResultAndError<bool> UpdateReadingDate(IEnumerable<KeyValuePair<string, int>> albaranes, DateTimeOffset readingDate);
    }
}