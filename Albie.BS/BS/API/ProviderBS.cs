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
    public class ProviderBS : EntityBS, IProviderBS
    {
        private readonly IConfiguration _conf;
        public ProviderBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET

        public CollectionList<Provider> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetProviderCount(filter, filterArr);

            if (total == 0) return new CollectionList<Provider>();

            var items = GetProviderList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<Provider>
            {
                Items = items,
                Total = total
            };
        }

        public int GetProviderCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<Provider> lista = db.Providers.WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<Provider> GetProviderList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<Provider> lista = db.Providers.WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn)
                                                    .OrderByAct("name", sortDescending);
            lista = lista.Include(o => o.ProviderCategory);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public Provider Get(string id)
        {
            return db.Providers
                .Include(o => o.Centro)
                .Include(o => o.Zona)
                .Include(o => o.InvoiceProviderDiscount)
                .Include(o => o.DiscountLineInvoice)
                .SingleOrDefault(o => o.VendorNo == id);
        }
        #endregion

        #region POST
        public ResultAndError<Provider> Add(Provider p)
        {
            ResultAndError<Provider> result = new ResultAndError<Provider>();
            try
            {
                db.Providers.Add(p);
                db.SaveChanges();
                return result.AddResult(p);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Provider> Update(Provider pr, bool insertIfNoExists = false)
        {
            ResultAndError<Provider> result = new ResultAndError<Provider>();
            try
            {
                Provider old = Get(pr.VendorNo);
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

        public bool UpdateMulti(IEnumerable<Provider> oProviders, bool insertIfNoExists = false)
        {
            foreach (Provider provider in oProviders)
            {
                Provider old = Get(provider.VendorNo);
                if (old == null && insertIfNoExists) Add(provider);
                else db.Entry(old).CurrentValues.SetValues(provider);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string providerNo)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                Provider oProvider = Get(providerNo);
                if (oProvider == null) return result.AddError("No se encontro el proveedor con el id " + providerNo);
                db.Providers.Remove(oProvider);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> Providers)
        {
            List<Provider> oProviders = new List<Provider>();
            foreach (string providerNo in Providers)
            {
                Provider oProvider = Get(providerNo);
                if (oProvider != null) oProviders.Add(oProvider);
            }
            db.Providers.RemoveRange(oProviders);
            db.SaveChanges();
            return true;
        }
        #endregion
    }
}
