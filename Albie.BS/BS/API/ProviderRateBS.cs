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
using System.Text;

namespace Albie.BS
{
    public class ProviderRateBS : EntityBS, IProviderRateBS
    {
        private readonly IConfiguration _conf;
        public ProviderRateBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<ProviderRate> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            return GetProviderRatesCollectionList();
        }
        public CollectionList<ProviderRate> GetProviderRatesCollectionList(string productId = "", bool hasProductIncluded = false, bool hasProviderIncluded = false, bool hasUnitMeasureProductIncluded = false, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            var total = GetProviderRateCount(productId, filter, filterArr);

            if (total == 0) return new CollectionList<ProviderRate>();

            var items = GetProviderRateList(productId, filter, filterArr, pageIndex, pagesize, sortName, sortDescending, hasProductIncluded, hasProviderIncluded, hasUnitMeasureProductIncluded);

            return new CollectionList<ProviderRate>
            {
                Items = items,
                Total = total
            };
        }

        public int GetProviderRateCount(string productId, string filter = "", List<FilterCriteria> filterArr = null, string fieldFilter = nameof(ProviderRate.Provider) + "." + nameof(ProviderRate.Provider.Name))
        {
            IQueryable<ProviderRate> lista = db.ProviderRates
                                            .Where(o => o.ProductNo == productId)
                                           .WhereAct(filterArr, filter, fieldFilter: fieldFilter, opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<ProviderRate> GetProviderRateList(string productId, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasProductIncluded = false, bool hasProviderIncluded = false, bool hasUnitMeasureProductIncluded = false)
        {
            IQueryable<ProviderRate> lista = db.ProviderRates
                                           .Where(o => o.ProductNo == productId)
                                           .WhereAct(filterArr, filter, fieldFilter: "VendorNo", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);


            if (hasProductIncluded) lista = lista.Include(o => o.Product).ThenInclude(o => o.UnitMeasureProduct);
            if (hasProviderIncluded) lista = lista.Include(o => o.Provider);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public ProviderRate Get(string productId, string providerId)
        {
            return db.ProviderRates.Include(o => o.Provider).SingleOrDefault(o => o.ProductNo == productId && o.VendorNo == providerId);
        }

        public ProviderRate Get(string id)
        {
            return Get("", "");
        }
        #endregion

        #region POST
        public ResultAndError<ProviderRate> Add(ProviderRate pr)
        {
            ResultAndError<ProviderRate> result = new ResultAndError<ProviderRate>();
            try
            {
                if (pr.Id == Guid.Empty) pr.Id = new Guid();
                db.ProviderRates.Add(pr);
                db.SaveChanges();
                return result.AddResult(pr);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<ProviderRate> Update(ProviderRate pr, bool insertIfNoExists = false)
        {
            ResultAndError<ProviderRate> result = new ResultAndError<ProviderRate>();
            try
            {
                ProviderRate old = Get(pr.ProductNo, pr.VendorNo);
                if (old == null && insertIfNoExists) return Add(pr);
                db.Entry(old).CurrentValues.SetValues(pr);
                db.SaveChanges();
                return result.AddResult(pr);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<ProviderRate> oProviderRates, bool insertIfNoExists = false)
        {
            foreach (ProviderRate ProviderRate in oProviderRates)
            {
                ProviderRate old = Get(ProviderRate.ProductNo, ProviderRate.VendorNo);
                if (old == null && insertIfNoExists) Add(ProviderRate);
                else db.Entry(old).CurrentValues.SetValues(ProviderRate);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            return Delete("", "");
        }

        public ResultAndError<bool> Delete(string productNo, string providerNo)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                ProviderRate oProviderRate = Get(productNo, providerNo);
                if (oProviderRate == null) return result.AddError("No se encontro la tarifa con el id " + productNo + " " + providerNo);
                db.ProviderRates.Remove(oProviderRate);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> ProviderRates)
        {
            List<ProviderRate> oProviderRates = new List<ProviderRate>();
            foreach (string ProviderRateNo in ProviderRates)
            {
                ProviderRate oProviderRate = Get(ProviderRateNo);
                if (oProviderRate != null) oProviderRates.Add(oProviderRate);
            }
            db.ProviderRates.RemoveRange(oProviderRates);
            db.SaveChanges();
            return true;
        }
        #endregion
    }
}
