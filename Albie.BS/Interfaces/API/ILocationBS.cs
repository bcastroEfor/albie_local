using ActioBP.Linq.FilterLinq;
using Albie.Models;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface ILocationBS : IEntityAlbieBS<Location>
    {
        IEnumerable<Location> GetWarehouseList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
    }
}