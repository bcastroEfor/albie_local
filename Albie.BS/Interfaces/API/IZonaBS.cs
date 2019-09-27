using System.Collections.Generic;
using ActioBP.Linq.FilterLinq;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface IZonaBS
    {
        IEnumerable<Zona> GetZonaList(string almacen = "", string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
    }
}