using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;
using System;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface ISalesCountedCenterBS : IEntityAlbieBS<SalesCountedCenter>
    {
        CollectionList<SalesCountedCenter> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "");
        ResultAndError<bool> UpdateReadingDate(IEnumerable<int> centersNo, DateTimeOffset readingDate);
        VentasCentro<Center> GetVentasCentro(int year = -1, int month = -1, int mode = 7);
    }
}