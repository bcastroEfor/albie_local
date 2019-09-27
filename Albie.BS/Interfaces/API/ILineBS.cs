using System;
using System.Collections.Generic;
using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface ILineBS
    {        
        CollectionList<Line> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
        CollectionList<Line> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "");
        ResultAndError<bool> UpdateReadingDate(IEnumerable<int> linesNo, DateTimeOffset readingDate);
        ResultAndError<Line> Update(Line cr, bool insertIfNoExists = false);
        ResultAndError<Line> Add(Line c);
    }
}