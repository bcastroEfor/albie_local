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
    public class SubCentreBS : EntityBS, ISubCentreBS
    {
        private readonly IConfiguration _conf;
        public SubCentreBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<Subcenter> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetSubcenterCount(filter, filterArr);

            if (total == 0) return new CollectionList<Subcenter>();

            var items = GetSubcenterList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<Subcenter>
            {
                Items = items,
                Total = total
            };
        }

        public int GetSubcenterCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<Subcenter> lista = db.Subcenters
                                           .WhereAct(filterArr, filter, fieldFilter: "code", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<Subcenter> GetSubcenterList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<Subcenter> lista = db.Subcenters
                                           .WhereAct(filterArr, filter, fieldFilter: "code", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public Subcenter Get(string id)
        {
            return db.Subcenters.Include(o => o.Center).Include(o => o.Centro).Include(o => o.Zona).SingleOrDefault(o => o.Code == id);
        }
        #endregion

        #region POST
        public ResultAndError<Subcenter> Add(Subcenter c)
        {
            ResultAndError<Subcenter> result = new ResultAndError<Subcenter>();
            try
            {
                db.Subcenters.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Subcenter> Update(Subcenter cr, bool insertIfNoExists = false)
        {
            ResultAndError<Subcenter> result = new ResultAndError<Subcenter>();
            try
            {
                Subcenter old = Get(cr.Code);
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

        public bool UpdateMulti(IEnumerable<Subcenter> oSubcenters, bool insertIfNoExists = false)
        {
            foreach (Subcenter Subcenter in oSubcenters)
            {
                Subcenter old = Get(Subcenter.Code);
                if (old == null && insertIfNoExists) Add(Subcenter);
                else db.Entry(old).CurrentValues.SetValues(Subcenter);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                Subcenter Subcenter = Get(id);
                if (Subcenter == null) return result.AddError("No se encontro el subcentro con el id " + id);
                db.Subcenters.Remove(Subcenter);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> Subcenters)
        {
            List<Subcenter> oSubcenters = new List<Subcenter>();
            foreach (string SubcenterNo in Subcenters)
            {
                Subcenter oSubcenter = Get(SubcenterNo);
                if (oSubcenter != null) oSubcenters.Add(oSubcenter);
            }
            db.Subcenters.RemoveRange(oSubcenters);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
