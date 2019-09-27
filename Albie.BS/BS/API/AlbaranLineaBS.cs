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
    public class AlbaranLineaBS : EntityBS, IAlbaranLineaBS
    {
        private readonly IConfiguration _conf;
        public AlbaranLineaBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<AlbaranLinea> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetAlbaranLineaCount(filter, filterArr);

            if (total == 0) return new CollectionList<AlbaranLinea>();

            var items = GetAlbaranLineaList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<AlbaranLinea>
            {
                Items = items,
                Total = total
            };
        }

        public CollectionList<AlbaranLinea> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {

            var total = GetAlbaranLineaCount(filter, filterArr, readingDate, filterReadingDate);

            if (total == 0) return new CollectionList<AlbaranLinea>();

            var items = GetAlbaranLineaList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, readingDate, filterReadingDate);

            return new CollectionList<AlbaranLinea>
            {
                Items = items,
                Total = total
            };
        }

        public int GetAlbaranLineaCount(string filter = "", List<FilterCriteria> filterArr = null, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<AlbaranLinea> lista = db.AlbaranLineas
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn);
            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            return lista.Count();
        }

        public IEnumerable<AlbaranLinea> GetAlbaranLineaList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<AlbaranLinea> lista = db.AlbaranLineas
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IQueryable<AlbaranLinea> FilterReadingDate(IQueryable<AlbaranLinea> AlbaranLineas, DateTimeOffset? readingDate, string readingDateFilter)
        {
            if (readingDateFilter == "<") return AlbaranLineas = AlbaranLineas.Where(o => o.ReadingDate < readingDate);
            else if (readingDateFilter == "<=") return AlbaranLineas = AlbaranLineas.Where(o => o.ReadingDate <= readingDate);
            else if (readingDateFilter == "=") return AlbaranLineas = AlbaranLineas.Where(o => o.ReadingDate == readingDate);
            else if (readingDateFilter == ">") return AlbaranLineas = AlbaranLineas.Where(o => o.ReadingDate > readingDate);
            else if (readingDateFilter == ">=") return AlbaranLineas = AlbaranLineas.Where(o => o.ReadingDate >= readingDate);
            return AlbaranLineas = AlbaranLineas.Where(o => o.ReadingDate != null);
        }

        public AlbaranLinea Get(int id, string albaranNo)
        {
            return db.AlbaranLineas
                .SingleOrDefault(o => o.No == id && o.AlbaranCompraNo == albaranNo);
        }
        #endregion

        #region POST
        public ResultAndError<AlbaranLinea> Add(AlbaranLinea c)
        {
            ResultAndError<AlbaranLinea> result = new ResultAndError<AlbaranLinea>();
            try
            {
                db.AlbaranLineas.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<AlbaranLinea> Update(AlbaranLinea cr, bool insertIfNoExists = false)
        {
            ResultAndError<AlbaranLinea> result = new ResultAndError<AlbaranLinea>();
            try
            {
                AlbaranLinea old = Get(cr.No, cr.AlbaranCompraNo);
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

        public ResultAndError<bool> UpdateReadingDate(IEnumerable<KeyValuePair<string, int>> albaranes, DateTimeOffset readingDate)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                foreach (KeyValuePair<string, int> albaran in albaranes)
                {
                    AlbaranLinea oAlbaranLinea = Get(albaran.Value, albaran.Key);
                    if (oAlbaranLinea == null) continue;
                    oAlbaranLinea.ReadingDate = readingDate;
                    db.SaveChanges();
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<AlbaranLinea> oAlbaranLineas, bool insertIfNoExists = false)
        {
            foreach (AlbaranLinea AlbaranLinea in oAlbaranLineas)
            {
                AlbaranLinea old = Get(AlbaranLinea.No, AlbaranLinea.AlbaranCompraNo);
                if (old == null && insertIfNoExists) Add(AlbaranLinea);
                else db.Entry(old).CurrentValues.SetValues(AlbaranLinea);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(int id, string albaranNo)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                AlbaranLinea AlbaranLinea = Get(id, albaranNo);
                if (AlbaranLinea == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.AlbaranLineas.Remove(AlbaranLinea);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<KeyValuePair<string, int>> AlbaranLineas)
        {
            List<AlbaranLinea> oAlbaranLineas = new List<AlbaranLinea>();
            foreach (KeyValuePair<string, int> AlbaranLineaNo in AlbaranLineas)
            {
                AlbaranLinea oAlbaranLinea = Get(AlbaranLineaNo.Value, AlbaranLineaNo.Key);
                if (oAlbaranLinea != null) oAlbaranLineas.Add(oAlbaranLinea);
            }
            db.AlbaranLineas.RemoveRange(oAlbaranLineas);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
