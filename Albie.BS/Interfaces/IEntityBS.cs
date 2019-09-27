using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using System;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface IEntityBS<T>
    {
        T Get(Guid id);
        CollectionList<T> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
        T Add(T m);
        T Update(T m);
        bool Delete(Guid id);
    }
}
