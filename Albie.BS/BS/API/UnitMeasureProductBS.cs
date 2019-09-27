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
    public class UnitMeasureProductBS : EntityBS, IUnitMeasureProductBS
    {

        private readonly IConfiguration _conf;
        public UnitMeasureProductBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<UnitMeasureProduct> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetUnitMeasureProductCount(filter, filterArr);

            if (total == 0) return new CollectionList<UnitMeasureProduct>();

            var items = GetUnitMeasureProductList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<UnitMeasureProduct>
            {
                Items = items,
                Total = total
            };
        }

        public int GetUnitMeasureProductCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<UnitMeasureProduct> lista = db.UnitMeasureProducts
                                           .WhereAct(filterArr, filter, fieldFilter: "code", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<UnitMeasureProduct> GetUnitMeasureProductList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<UnitMeasureProduct> lista = db.UnitMeasureProducts
                                           .WhereAct(filterArr, filter, fieldFilter: "code", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public UnitMeasureProduct Get(string id)
        {
            return db.UnitMeasureProducts.SingleOrDefault(o => o.ProductNo == id);
        }
        #endregion

        #region POST
        public ResultAndError<UnitMeasureProduct> Add(UnitMeasureProduct c)
        {
            ResultAndError<UnitMeasureProduct> result = new ResultAndError<UnitMeasureProduct>();
            try
            {
                db.UnitMeasureProducts.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<UnitMeasureProduct> Update(UnitMeasureProduct u, bool insertIfNoExists = false)
        {
            ResultAndError<UnitMeasureProduct> result = new ResultAndError<UnitMeasureProduct>();
            try
            {
                UnitMeasureProduct old = Get(u.Code);
                if (old == null && insertIfNoExists) return Add(u);
                db.Entry(old).CurrentValues.SetValues(u);
                db.SaveChanges();
                return result.AddResult(u);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<UnitMeasureProduct> oUnitMeasureProducts, bool insertIfNoExists = false)
        {
            foreach (UnitMeasureProduct UnitMeasureProduct in oUnitMeasureProducts)
            {
                UnitMeasureProduct old = Get(UnitMeasureProduct.Code);
                if (old == null && insertIfNoExists) Add(UnitMeasureProduct);
                else db.Entry(old).CurrentValues.SetValues(UnitMeasureProduct);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                UnitMeasureProduct UnitMeasureProduct = Get(id);
                if (UnitMeasureProduct == null) return result.AddError("No se encontro el subcentro con el id " + id);
                db.UnitMeasureProducts.Remove(UnitMeasureProduct);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> UnitMeasureProducts)
        {
            List<UnitMeasureProduct> oUnitMeasureProducts = new List<UnitMeasureProduct>();
            foreach (string UnitMeasureProductNo in UnitMeasureProducts)
            {
                UnitMeasureProduct oUnitMeasureProduct = Get(UnitMeasureProductNo);
                if (oUnitMeasureProduct != null) oUnitMeasureProducts.Add(oUnitMeasureProduct);
            }
            db.UnitMeasureProducts.RemoveRange(oUnitMeasureProducts);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
