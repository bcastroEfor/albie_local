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
    public class LocationBS : EntityBS, ILocationBS
    {
        private readonly IConfiguration _conf;
        public LocationBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<Location> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetWarehouseCount(filter, filterArr);

            if (total == 0) return new CollectionList<Location>();

            var items = GetWarehouseList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<Location>
            {
                Items = items,
                Total = total
            };
        }

        public int GetWarehouseCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<Location> lista = db.Locations
                                           .WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<Location> GetWarehouseList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<Location> lista = db.Locations
                                           .WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn)
                                           .OrderByAct("name", sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public Location Get(string id)
        {
            return db.Locations.SingleOrDefault(o => o.Code == id);
        }
        #endregion

        #region POST
        public ResultAndError<Location> Add(Location c)
        {
            ResultAndError<Location> result = new ResultAndError<Location>();
            try
            {
                db.Locations.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Location> Update(Location c, bool insertIfNoExists = false)
        {
            ResultAndError<Location> result = new ResultAndError<Location>();
            try
            {
                Location old = Get(c.Code);
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

        public bool UpdateMulti(IEnumerable<Location> oWarehouses, bool insertIfNoExists = false)
        {
            foreach (Location Warehouse in oWarehouses)
            {
                Location old = Get(Warehouse.Code);
                if (old == null && insertIfNoExists) Add(Warehouse);
                else db.Entry(old).CurrentValues.SetValues(Warehouse);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                Location Warehouse = Get(id);
                if (Warehouse == null) return result.AddError("No se encontro el almacen con el id " + id);
                db.Locations.Remove(Warehouse);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> Warehouses)
        {
            List<Location> oWarehouses = new List<Location>();
            foreach (string WarehouseNo in Warehouses)
            {
                Location oWarehouse = Get(WarehouseNo);
                if (oWarehouse != null) oWarehouses.Add(oWarehouse);
            }
            db.Locations.RemoveRange(oWarehouses);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
