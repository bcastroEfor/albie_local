using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Albie.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Albie.BS
{
    public class ProductBS : EntityBS, IProductBS
    {
        private readonly IConfiguration _conf;
        public ProductBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET

        public CollectionList<ProductCategory> GetProductCategoryCollectionList(string parentCategory, bool hasUnitMeasureProductIncluded = false, bool hasProductIncluded = false, bool hasProviderCategoryIncluded = false, bool hasProviderIncluded = false, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetProductCategoryCount(filter, filterArr, parentCategory);

            if (total == 0) return new CollectionList<ProductCategory>();

            var items = GetProductCategoryList(parentCategory, filter, filterArr, pageIndex, pagesize, sortName, sortDescending, hasUnitMeasureProductIncluded, hasProductIncluded, hasProviderCategoryIncluded, hasProviderIncluded);

            return new CollectionList<ProductCategory>
            {
                Items = items,
                Total = total
            };
        }

        public int GetProductCategoryCount(string filter = "", List<FilterCriteria> filterArr = null, string parentCategory = "")
        {
            IQueryable<ProductCategory> lista = db.ProductCategories
                                                  .Where(o => o.ParentCategory == parentCategory)
                                                  .WhereAct(filterArr, filter, fieldFilter: "description", opFilter: FilterOperator.Cn);

            return lista.Count();
        }

        public IEnumerable<ProductCategory> GetProductCategoryList(string parentCategory, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasUnitMeasureProductIncluded = false, bool hasProductIncluded = false, bool hasProviderCategoryIncluded = false, bool hasProviderIncluded = false)
        {
            IQueryable<ProductCategory> lista = db.ProductCategories
                                                  .WhereAct(filterArr, filter, fieldFilter: "description", opFilter: FilterOperator.Cn)
                                                  .OrderByAct("description", sortDescending);

            if (string.IsNullOrEmpty(parentCategory)) lista = lista.Where(o => o.ParentCategory == parentCategory);
            if (hasUnitMeasureProductIncluded) lista = lista.Include(o => o.Product).ThenInclude(o => o.UnitMeasureProduct);
            else if (hasProductIncluded) lista = lista.Include(o => o.Product);
            //if (hasProviderIncluded) lista = lista.Include(o => o.ProviderCategory).ThenInclude(o => o.Provider);
            //else if (hasProviderCategoryIncluded) lista = lista.Include(o => o.ProviderCategory);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public CollectionList<Product> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            var total = GetProductAlmacenCount(filter, filterArr);

            if (total == 0) return new CollectionList<Product>();

            var items = GetProductAlmacenList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<Product>
            {
                Items = items,
                Total = total
            };
        }

        public int GetProductAlmacenCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<Product> lista = db.Products
                                                  .WhereAct(filterArr, filter, fieldFilter: "description", opFilter: FilterOperator.Cn);

            if (Convert.ToBoolean(filter)) lista = lista.Where(o => !db.AlmacenZPs.Select(x => x.ProductNo).Contains(o.ProductNo));
            else lista = lista.Where(o => db.AlmacenZPs.Select(x => x.ProductNo).Contains(o.ProductNo));

            return lista.Count();
        }

        public IEnumerable<Product> GetProductAlmacenList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<Product> lista = db.Products
                                                   .Include(o => o.AlmacenZP).ThenInclude(o => o.Zonas)
                                                   .Include(o => o.AlmacenZP).ThenInclude(o => o.Location)
                                                   .WhereAct(filterArr, filter, fieldFilter: "description", opFilter: FilterOperator.Cn)                                                   
                                                   .OrderByAct("description", sortDescending);

            if (Convert.ToBoolean(filter)) lista = lista.Where(o => !db.AlmacenZPs.Select(x => x.ProductNo).Contains(o.ProductNo));
            else lista = lista.Where(o => db.AlmacenZPs.Select(x => x.ProductNo).Contains(o.ProductNo));

            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public CollectionList<Product> GetCollectionListProducts(string productCategory = "", bool hasUnitMeasureProductIncluded = false, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetProductCount(filter, filterArr, productCategory);

            if (total == 0) return new CollectionList<Product>();

            var items = GetProductList(productCategory, filter, filterArr, pageIndex, pagesize, sortName, sortDescending, hasUnitMeasureProductIncluded);

            return new CollectionList<Product>
            {
                Items = items,
                Total = total
            };
        }

        public int GetProductCount(string filter = "", List<FilterCriteria> filterArr = null, string productCategory = "")
        {
            IQueryable<Product> lista = db.Products
                                                  .Where(o => o.ProductCategoryCode == productCategory)
                                                  .WhereAct(filterArr, filter, fieldFilter: "description", opFilter: FilterOperator.Cn);

            return lista.Count();
        }

        public IEnumerable<Product> GetProductList(string productCategory = "", string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasUnitMeasureProductIncluded = false, bool hasProductCategoryIncluded = false, string filterDescription = "")
        {
            IQueryable<Product> lista = db.Products.WhereAct(filterArr, filter, fieldFilter: "description", opFilter: FilterOperator.Cn)
                                                    .OrderByAct("description", sortDescending);

            if (!string.IsNullOrEmpty(productCategory) && (productCategory.Contains("PESC") || productCategory.Contains("FRUT") || productCategory.Contains("BEBIDAS"))) lista = lista.Where(o => o.ProductNo.Contains(productCategory.Substring(0, 7)));
            else if (!string.IsNullOrEmpty(productCategory)) lista = lista.Where(o => o.ProductCategoryCode.Contains(productCategory));
            if (!string.IsNullOrEmpty(filterDescription)) lista = lista.Where(o => o.Description.Contains(filterDescription));
            if (hasUnitMeasureProductIncluded) lista = lista.Include(o => o.UnitMeasureProduct);
            if (hasProductCategoryIncluded) lista = lista.Include(o => o.ProductCategory);
            lista = lista.Include(o => o.ProviderRates);

            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public Product Get(string productNo)
        {
            return db.Products
                             .Include(o => o.UnitMeasureProduct)
                             .Include(o => o.ProductCategory)
                             .SingleOrDefault(o => o.ProductNo == productNo);
        }

        public IEnumerable<Product> GetProductsList(string productCategory = "", bool hasUnitMeasureProductIncluded = false, bool hasProductCategoryIncluded = false)
        {
            IQueryable<Product> oProducts = db.Products;

            if (!string.IsNullOrEmpty(productCategory)) oProducts.Where(o => o.ProductCategoryCode.Contains(productCategory));
            if (hasUnitMeasureProductIncluded) oProducts = oProducts.Include(o => o.UnitMeasureProduct);
            if (hasProductCategoryIncluded) oProducts = oProducts.Include(o => o.ProductCategory);

            return oProducts;
        }

        #endregion

        #region POST
        public ResultAndError<Product> Add(Product p)
        {
            ResultAndError<Product> result = new ResultAndError<Product>();
            try
            {
                db.Products.Add(p);
                db.SaveChanges();
                return result.AddResult(p);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Product> Update(Product product, bool insertIfNoExists = false)
        {
            ResultAndError<Product> result = new ResultAndError<Product>();
            try
            {
                Product old = Get(product.ProductNo);
                if (old == null && insertIfNoExists) return Add(product);
                db.Entry(old).CurrentValues.SetValues(product);
                db.SaveChanges();
                return result.AddResult(product);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<Product> oProducts, bool insertIfNoExists = false)
        {
            foreach (Product product in oProducts)
            {
                Product old = Get(product.ProductNo);
                if (old == null && insertIfNoExists) Add(product);
                else db.Entry(old).CurrentValues.SetValues(product);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string productNo)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                Product oProduct = Get(productNo);
                if (oProduct == null) return result.AddError("No se encontro el producto con el id " + productNo);
                db.Products.Remove(oProduct);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> products)
        {
            List<Product> oProducts = new List<Product>();
            foreach (string productNo in products)
            {
                Product oProduct = Get(productNo);
                if (oProduct != null) oProducts.Add(oProduct);
            }
            db.Products.RemoveRange(oProducts);
            db.SaveChanges();
            return true;
        }       
        #endregion
    }
}
