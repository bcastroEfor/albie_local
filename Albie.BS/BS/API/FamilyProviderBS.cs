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
    public class FamilyProviderBS : EntityBS, IFamilyProviderBS
    {
        private readonly IConfiguration _conf;
        public FamilyProviderBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<FamilyProvider> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetFamilyProviderCount(filter, filterArr);

            if (total == 0) return new CollectionList<FamilyProvider>();

            var items = GetFamilyProviderList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<FamilyProvider>
            {
                Items = items,
                Total = total
            };
        }

        public int GetFamilyProviderCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<FamilyProvider> lista = db.FamilyProviders
                                           .WhereAct(filterArr, filter, fieldFilter: "code", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<FamilyProvider> GetFamilyProviderList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<FamilyProvider> lista = db.FamilyProviders
                                           .WhereAct(filterArr, filter, fieldFilter: "code", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public FamilyProvider Get(string id)
        {
            return db.FamilyProviders.SingleOrDefault(o => o.VendorNo == id);
        }
        #endregion

        #region POST
        public ResultAndError<FamilyProvider> Add(FamilyProvider c)
        {
            ResultAndError<FamilyProvider> result = new ResultAndError<FamilyProvider>();
            try
            {
                db.FamilyProviders.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<FamilyProvider> Update(FamilyProvider cr, bool insertIfNoExists = false)
        {
            ResultAndError<FamilyProvider> result = new ResultAndError<FamilyProvider>();
            try
            {
                FamilyProvider old = Get(cr.VendorNo);
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

        public bool UpdateMulti(IEnumerable<FamilyProvider> oFamilyProviders, bool insertIfNoExists = false)
        {
            foreach (FamilyProvider FamilyProvider in oFamilyProviders)
            {
                FamilyProvider old = Get(FamilyProvider.VendorNo);
                if (old == null && insertIfNoExists) Add(FamilyProvider);
                else db.Entry(old).CurrentValues.SetValues(FamilyProvider);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                FamilyProvider FamilyProvider = Get(id);
                if (FamilyProvider == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.FamilyProviders.Remove(FamilyProvider);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> FamilyProviders)
        {
            List<FamilyProvider> oFamilyProviders = new List<FamilyProvider>();
            foreach (string FamilyProviderNo in FamilyProviders)
            {
                FamilyProvider oFamilyProvider = Get(FamilyProviderNo);
                if (oFamilyProvider != null) oFamilyProviders.Add(oFamilyProvider);
            }
            db.FamilyProviders.RemoveRange(oFamilyProviders);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
