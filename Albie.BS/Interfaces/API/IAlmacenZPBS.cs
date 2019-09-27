using System.Collections.Generic;
using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface IAlmacenZPBS : IEntityAlbieBS<AlmacenZP>
    {        
        ResultAndError<bool> Delete(string productNo, string locationCode);
        AlmacenZP Get(string productId, string location);
        ResultAndError<AlmacenZP> AddProductToAlmacen(Product product, string almacen, string zona);
        CollectionList<AlmacenZP> GetCollectionListAlmacenes(string almacen = "", string zona = "", string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
    }
}