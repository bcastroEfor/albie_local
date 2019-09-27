using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;
using System;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface IAlbaranCompraBS : IEntityAlbieBS<AlbaranCompra>
    {
        CollectionList<AlbaranCompra> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "");
        ResultAndError<bool> UpdateReadingDate(IEnumerable<string> centersNo, DateTimeOffset readingDate);
        void RecepcionMercancia(Document oOrder, DateTimeOffset albaranDate, bool nonConform);
        void AnularAlbaran(AlbaranCompra albaran);
    }
}