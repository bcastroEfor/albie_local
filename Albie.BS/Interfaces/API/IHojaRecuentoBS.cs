using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;
using System.Collections.Generic;
using System.Linq;

namespace Albie.BS.Interfaces
{
    public interface IHojaRecuentoBS : IEntityAlbieBS<HojaRecuento, int>
    {
        ResultAndError<HojaRecuento> AddProductToAlmacen(Product product, string almacen, string zona);
        ResultAndError<Recuento> IniciarRecuento(string center, string almacen, string product, string zona);
        List<HojaRecuentoGroup> GetHojaRecuentoByCodigo();
        CollectionList<HojaRecuento> GetCollectionList(string idRecuento, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
    }
}