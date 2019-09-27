using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Models;
using System;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface IListBS
    {
        List<ProductList> PostList(List<Product> products, string listName);
        CollectionList<ProductList> GetCartProductListsCollectionList(Guid? listaId = null, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasProductIncluded = false, bool hasProviderRateIncluded = false);
        IEnumerable<ProductList> GetProductListByListId(Guid listId, bool hasProductIncluded = false);
        bool ChangeProductRate(ProviderRate rate, Guid productId);
    }
}
