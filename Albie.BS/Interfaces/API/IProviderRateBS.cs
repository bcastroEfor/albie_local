using System;
using System.Collections.Generic;
using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface IProviderRateBS : IEntityAlbieBS<ProviderRate>
    {
        #region GET
        CollectionList<ProviderRate> GetProviderRatesCollectionList(string productId, bool hasProductIncluded = false, bool hasProviderIncluded = false, bool hasUnitMeasureProductIncluded = false, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
        ResultAndError<bool> Delete(string productNo, string providerNo);
        ProviderRate Get(string productId, string providerId);
        #endregion
    }
}