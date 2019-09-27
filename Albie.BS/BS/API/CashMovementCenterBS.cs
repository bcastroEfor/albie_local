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
    public class CashMovementCenterBS : EntityBS, ICashMovementCenterBS
    {
        private readonly IConfiguration _conf;
        public CashMovementCenterBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<CashMovementCenter> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetCashMovementCenterCount(filter, filterArr);

            if (total == 0) return new CollectionList<CashMovementCenter>();

            var items = GetCashMovementCenterList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<CashMovementCenter>
            {
                Items = items,
                Total = total
            };
        }

        public CollectionList<CashMovementCenter> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {

            var total = GetCashMovementCenterCount(filter, filterArr, readingDate, filterReadingDate);

            if (total == 0) return new CollectionList<CashMovementCenter>();

            var items = GetCashMovementCenterList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, readingDate, filterReadingDate);

            return new CollectionList<CashMovementCenter>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCashMovementCenterCount(string filter = "", List<FilterCriteria> filterArr = null, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<CashMovementCenter> lista = db.CashMovementCenters
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn);

            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            return lista.Count();
        }

        public IEnumerable<CashMovementCenter> GetCashMovementCenterList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<CashMovementCenter> lista = db.CashMovementCenters
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IQueryable<CashMovementCenter> FilterReadingDate(IQueryable<CashMovementCenter> salesCenters, DateTimeOffset? readingDate, string readingDateFilter)
        {
            if (readingDateFilter == "<") return salesCenters = salesCenters.Where(o => o.ReadingDate < readingDate);
            else if (readingDateFilter == "<=") return salesCenters = salesCenters.Where(o => o.ReadingDate <= readingDate);
            else if (readingDateFilter == "=") return salesCenters = salesCenters.Where(o => o.ReadingDate == readingDate);
            else if (readingDateFilter == ">") return salesCenters = salesCenters.Where(o => o.ReadingDate > readingDate);
            else if (readingDateFilter == ">=") return salesCenters = salesCenters.Where(o => o.ReadingDate >= readingDate);
            return salesCenters = salesCenters.Where(o => o.ReadingDate != null);
        }

        public CashMovementCenter Get(string id)
        {
            return db.CashMovementCenters.Include(o => o.Center).SingleOrDefault(o => o.EntryNo == Convert.ToInt32(id));
        }
        #endregion

        #region POST
        public ResultAndError<CashMovementCenter> Add(CashMovementCenter c)
        {
            ResultAndError<CashMovementCenter> result = new ResultAndError<CashMovementCenter>();
            try
            {
                db.CashMovementCenters.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<CashMovementCenter> Update(CashMovementCenter cr, bool insertIfNoExists = false)
        {
            ResultAndError<CashMovementCenter> result = new ResultAndError<CashMovementCenter>();
            try
            {
                CashMovementCenter old = Get(cr.EntryNo.ToString());
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

        public ResultAndError<bool> UpdateReadingDate(IEnumerable<int> cashId, DateTimeOffset readingDate)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                foreach (int no in cashId)
                {
                    CashMovementCenter oCash = Get(no.ToString());
                    oCash.ReadingDate = readingDate;
                    db.SaveChanges();
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<CashMovementCenter> oCashMovementCenters, bool insertIfNoExists = false)
        {
            foreach (CashMovementCenter CashMovementCenter in oCashMovementCenters)
            {
                CashMovementCenter old = Get(CashMovementCenter.EntryNo.ToString());
                if (old == null && insertIfNoExists) Add(CashMovementCenter);
                else db.Entry(old).CurrentValues.SetValues(CashMovementCenter);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                CashMovementCenter CashMovementCenter = Get(id);
                if (CashMovementCenter == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.CashMovementCenters.Remove(CashMovementCenter);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> CashMovementCenters)
        {
            List<CashMovementCenter> oCashMovementCenters = new List<CashMovementCenter>();
            foreach (string CashMovementCenterNo in CashMovementCenters)
            {
                CashMovementCenter oCashMovementCenter = Get(CashMovementCenterNo);
                if (oCashMovementCenter != null) oCashMovementCenters.Add(oCashMovementCenter);
            }
            db.CashMovementCenters.RemoveRange(oCashMovementCenters);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
