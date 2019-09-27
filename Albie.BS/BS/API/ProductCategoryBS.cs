using ActioBP.General.HttpModels;
using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Albie.Repository.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Albie.BS
{
    public class ProductCategoryBS : EntityBS, IProductCategoryBS
    {

        private readonly IConfiguration _conf;
        public ProductCategoryBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<ProductCategory> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetProductCategoryCount(filter, filterArr);

            if (total == 0) return new CollectionList<ProductCategory>();

            var items = GetProductCategoryList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<ProductCategory>
            {
                Items = items,
                Total = total
            };
        }

        public int GetProductCategoryCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<ProductCategory> lista = db.ProductCategories
                                           .WhereAct(filterArr, filter, fieldFilter: "code", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<ProductCategory> GetProductCategoryList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<ProductCategory> lista = db.ProductCategories
                                           .WhereAct(filterArr, filter, fieldFilter: "code", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public ProductCategory Get(string id)
        {
            return db.ProductCategories.SingleOrDefault(o => o.Code == id);
        }
        #endregion

        #region POST
        public ResultAndError<ProductCategory> Add(ProductCategory c)
        {
            ResultAndError<ProductCategory> result = new ResultAndError<ProductCategory>();
            try
            {
                db.ProductCategories.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<ProductCategory> Update(ProductCategory cr, bool insertIfNoExists = false)
        {
            ResultAndError<ProductCategory> result = new ResultAndError<ProductCategory>();
            try
            {
                ProductCategory old = Get(cr.Code);
                if (old == null && insertIfNoExists) return Add(cr);
                db.Entry(old).CurrentValues.SetValues(cr);
                db.SaveChanges();
                return result.AddResult(cr);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<ProductCategory> oProductCategories, bool insertIfNoExists = false)
        {
            foreach (ProductCategory ProductCategory in oProductCategories)
            {
                ProductCategory old = Get(ProductCategory.Code);
                if (old == null && insertIfNoExists) Add(ProductCategory);
                else db.Entry(old).CurrentValues.SetValues(ProductCategory);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                ProductCategory ProductCategory = Get(id);
                if (ProductCategory == null) return result.AddError("No se encontro la categoria con el id " + id);
                db.ProductCategories.Remove(ProductCategory);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> ProductCategories)
        {
            List<ProductCategory> oProductCategories = new List<ProductCategory>();
            foreach (string ProductCategoryNo in ProductCategories)
            {
                ProductCategory oProductCategory = Get(ProductCategoryNo);
                if (oProductCategory != null) oProductCategories.Add(oProductCategory);
            }
            db.ProductCategories.RemoveRange(oProductCategories);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
