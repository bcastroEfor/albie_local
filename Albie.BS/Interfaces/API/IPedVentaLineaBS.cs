using System;
using System.Collections.Generic;
using System.Linq;
using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface IPedVentaLineaBS
    {
        ResultAndError<PedVentaLinea> Add(PedVentaLinea c);
        ResultAndError<bool> Delete(string documentNo, int lineNo);
        bool DeleteMulti(IEnumerable<KeyValuePair<string, int>> PedVentaLineas);
        PedVentaLinea Get(string documentNo, int lineNo);
        CollectionList<PedVentaLinea> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
        CollectionList<PedVentaLinea> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "");
        ResultAndError<PedVentaLinea> Update(PedVentaLinea cr, bool insertIfNoExists = false);
        bool UpdateMulti(IEnumerable<PedVentaLinea> oPedVentaLineas, bool insertIfNoExists = false);
        ResultAndError<bool> UpdateReadingDate(IEnumerable<KeyValuePair<string, int>> centersNo, DateTimeOffset readingDate);
    }
}