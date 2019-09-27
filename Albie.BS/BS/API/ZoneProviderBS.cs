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
    public class ZoneProviderBS : EntityBS, IZoneProviderBS
    {
        private readonly IConfiguration _conf;
        public ZoneProviderBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<ZoneProvider> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetZoneProviderCount(filter, filterArr);

            if (total == 0) return new CollectionList<ZoneProvider>();

            var items = GetZoneProviderList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<ZoneProvider>
            {
                Items = items,
                Total = total
            };
        }

        public int GetZoneProviderCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<ZoneProvider> lista = db.ZoneProviders
                                           .WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<ZoneProvider> GetZoneProviderList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<ZoneProvider> lista = db.ZoneProviders
                                           .WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public ZoneProvider Get(string id)
        {
            return db.ZoneProviders.Include(o => o.Centro).Include(o => o.Provider).SingleOrDefault(o => o.VendorNo == id);
        }
        #endregion

        #region POST
        public ResultAndError<ZoneProvider> Add(ZoneProvider c)
        {
            ResultAndError<ZoneProvider> result = new ResultAndError<ZoneProvider>();
            try
            {
                db.ZoneProviders.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<ZoneProvider> Update(ZoneProvider c, bool insertIfNoExists = false)
        {
            ResultAndError<ZoneProvider> result = new ResultAndError<ZoneProvider>();
            try
            {
                ZoneProvider old = Get(c.Code);
                if (old == null && insertIfNoExists) return Add(c);
                db.Entry(old).CurrentValues.SetValues(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<ZoneProvider> oZoneProviders, bool insertIfNoExists = false)
        {
            foreach (ZoneProvider ZoneProvider in oZoneProviders)
            {
                ZoneProvider old = Get(ZoneProvider.Code);
                if (old == null && insertIfNoExists) Add(ZoneProvider);
                else db.Entry(old).CurrentValues.SetValues(ZoneProvider);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                ZoneProvider ZoneProvider = Get(id);
                if (ZoneProvider == null) return result.AddError("No se encontro la zona del proveedor con el id " + id);
                db.ZoneProviders.Remove(ZoneProvider);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> ZoneProviders)
        {
            List<ZoneProvider> oZoneProviders = new List<ZoneProvider>();
            foreach (string ZoneProviderNo in ZoneProviders)
            {
                ZoneProvider oZoneProvider = Get(ZoneProviderNo);
                if (oZoneProvider != null) oZoneProviders.Add(oZoneProvider);
            }
            db.ZoneProviders.RemoveRange(oZoneProviders);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
