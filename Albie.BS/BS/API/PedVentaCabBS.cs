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
    public class PedVentaCabBS : EntityBS, IPedVentaCabBS
    {
        private readonly IConfiguration _conf;
        public PedVentaCabBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<PedVentaCab> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetPedVentaCabsCount(filter, filterArr);

            if (total == 0) return new CollectionList<PedVentaCab>();

            var items = GetPedVentaCabsList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<PedVentaCab>
            {
                Items = items,
                Total = total
            };
        }

        public CollectionList<PedVentaCab> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {

            var total = GetPedVentaCabsCount(filter, filterArr, readingDate, filterReadingDate);

            if (total == 0) return new CollectionList<PedVentaCab>();

            var items = GetPedVentaCabsList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, readingDate, filterReadingDate);

            return new CollectionList<PedVentaCab>
            {
                Items = items,
                Total = total
            };
        }

        public int GetPedVentaCabsCount(string filter = "", List<FilterCriteria> filterArr = null, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<PedVentaCab> lista = db.PedVentaCabs
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn);
            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            return lista.Count();
        }

        public IEnumerable<PedVentaCab> GetPedVentaCabsList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<PedVentaCab> lista = db.PedVentaCabs
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IQueryable<PedVentaCab> FilterReadingDate(IQueryable<PedVentaCab> PedVentaCabss, DateTimeOffset? readingDate, string readingDateFilter)
        {
            if (readingDateFilter == "<") return PedVentaCabss = PedVentaCabss.Where(o => o.ReadingDate < readingDate);
            else if (readingDateFilter == "<=") return PedVentaCabss = PedVentaCabss.Where(o => o.ReadingDate <= readingDate);
            else if (readingDateFilter == "=") return PedVentaCabss = PedVentaCabss.Where(o => o.ReadingDate == readingDate);
            else if (readingDateFilter == ">") return PedVentaCabss = PedVentaCabss.Where(o => o.ReadingDate > readingDate);
            else if (readingDateFilter == ">=") return PedVentaCabss = PedVentaCabss.Where(o => o.ReadingDate >= readingDate);
            return PedVentaCabss = PedVentaCabss.Where(o => o.ReadingDate != null);
        }

        public PedVentaCab Get(string id)
        {
            return db.PedVentaCabs
                .SingleOrDefault(o => o.No == id);
        }
        #endregion

        #region POST
        public ResultAndError<PedVentaCab> Add(PedVentaCab c)
        {
            ResultAndError<PedVentaCab> result = new ResultAndError<PedVentaCab>();
            try
            {
                db.PedVentaCabs.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<PedVentaCab> Update(PedVentaCab cr, bool insertIfNoExists = false)
        {
            ResultAndError<PedVentaCab> result = new ResultAndError<PedVentaCab>();
            try
            {
                PedVentaCab old = Get(cr.No);
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

        public ResultAndError<bool> UpdateReadingDate(IEnumerable<string> centersNo, DateTimeOffset readingDate)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                foreach (string no in centersNo)
                {
                    PedVentaCab oPedVentaCabs = Get(no);
                    oPedVentaCabs.ReadingDate = readingDate;
                    db.SaveChanges();
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<PedVentaCab> oPedVentaCabs, bool insertIfNoExists = false)
        {
            foreach (PedVentaCab albaran in oPedVentaCabs)
            {
                PedVentaCab old = Get(albaran.No);
                if (old == null && insertIfNoExists) Add(albaran);
                else db.Entry(old).CurrentValues.SetValues(albaran);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                PedVentaCab PedVentaCabs = Get(id);
                if (PedVentaCabs == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.PedVentaCabs.Remove(PedVentaCabs);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> PedVentaCabs)
        {
            List<PedVentaCab> oAlbaran = new List<PedVentaCab>();
            foreach (string PedVentaCabsNo in PedVentaCabs)
            {
                PedVentaCab oPedVentaCabs = Get(PedVentaCabsNo);
                if (oPedVentaCabs != null) oAlbaran.Add(oPedVentaCabs);
            }
            db.PedVentaCabs.RemoveRange(oAlbaran);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
