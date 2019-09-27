using System;
using System.Collections.Generic;
using System.Linq;
using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface IPedVentaCabBS : IEntityAlbieBS<PedVentaCab>
    {
        IQueryable<PedVentaCab> FilterReadingDate(IQueryable<PedVentaCab> PedVentaCabss, DateTimeOffset? readingDate, string readingDateFilter);
        CollectionList<PedVentaCab> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "");
        ResultAndError<bool> UpdateReadingDate(IEnumerable<string> centersNo, DateTimeOffset readingDate);
    }
}