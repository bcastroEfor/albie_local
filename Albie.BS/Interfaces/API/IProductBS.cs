using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Models;
using System.Collections.Generic;

namespace Albie.BS.Interfaces
{
    public interface IProductBS : IEntityAlbieBS<Product>
    {
        CollectionList<ProductCategory> GetProductCategoryCollectionList(string productCategory = "", bool hasUnitMeasureProductIncluded = false, bool hasProductIncluded = false, bool hasProviderCategoryIncluded = false, bool hasProviderIncluded = false, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
        CollectionList<Product> GetCollectionListProducts(string productCategory, bool hasUnitMeasureProductIncluded = false, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false);
        IEnumerable<ProductCategory> GetProductCategoryList(string parentCategory = null, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasUnitMeasureProductIncluded = false, bool hasProductIncluded = false, bool hasProviderCategoryIncluded = false, bool hasProviderIncluded = false);
        IEnumerable<Product> GetProductList(string productCategory = "", string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasUnitMeasureProductIncluded = false, bool hasProductCategoryIncluded = false, string filterDescription = "");
        IEnumerable<Product> GetProductsList(string productCategory = "", bool hasUnitMeasureProductIncluded = false, bool hasProductCategoryIncluded = false);
    }
}
