using System;
using System.Collections.Generic;
using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface ICartBS
    {
        CollectionList<CartList> GetCartListsCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasListProductIncluded = false, bool pedidoHabitual = false);
        CartList GetListById(Guid id);
        bool SetAsUsualOrder(Guid idLista);
        CartList CreateList(string name);
    }
}