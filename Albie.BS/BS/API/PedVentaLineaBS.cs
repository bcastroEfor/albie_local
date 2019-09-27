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
    public class PedVentaLineaBS : EntityBS, IPedVentaLineaBS
    {
        private readonly IConfiguration _conf;
        public PedVentaLineaBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<PedVentaLinea> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetPedVentaLineasCount(filter, filterArr);

            if (total == 0) return new CollectionList<PedVentaLinea>();

            var items = GetPedVentaLineasList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<PedVentaLinea>
            {
                Items = items,
                Total = total
            };
        }

        public CollectionList<PedVentaLinea> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {

            var total = GetPedVentaLineasCount(filter, filterArr, readingDate, filterReadingDate);

            if (total == 0) return new CollectionList<PedVentaLinea>();

            var items = GetPedVentaLineasList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, readingDate, filterReadingDate);

            return new CollectionList<PedVentaLinea>
            {
                Items = items,
                Total = total
            };
        }

        public int GetPedVentaLineasCount(string filter = "", List<FilterCriteria> filterArr = null, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<PedVentaLinea> lista = db.PedVentaLineas
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn);
            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            return lista.Count();
        }

        public IEnumerable<PedVentaLinea> GetPedVentaLineasList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<PedVentaLinea> lista = db.PedVentaLineas
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IQueryable<PedVentaLinea> FilterReadingDate(IQueryable<PedVentaLinea> PedVentaLineass, DateTimeOffset? readingDate, string readingDateFilter)
        {
            if (readingDateFilter == "<") return PedVentaLineass = PedVentaLineass.Where(o => o.ReadingDate < readingDate);
            else if (readingDateFilter == "<=") return PedVentaLineass = PedVentaLineass.Where(o => o.ReadingDate <= readingDate);
            else if (readingDateFilter == "=") return PedVentaLineass = PedVentaLineass.Where(o => o.ReadingDate == readingDate);
            else if (readingDateFilter == ">") return PedVentaLineass = PedVentaLineass.Where(o => o.ReadingDate > readingDate);
            else if (readingDateFilter == ">=") return PedVentaLineass = PedVentaLineass.Where(o => o.ReadingDate >= readingDate);
            return PedVentaLineass = PedVentaLineass.Where(o => o.ReadingDate != null);
        }

        public PedVentaLinea Get(string documentNo, int lineNo)
        {
            return db.PedVentaLineas
                .SingleOrDefault(o => o.DocumentNo == documentNo && o.LineNo == lineNo);
        }
        #endregion

        #region POST
        public ResultAndError<PedVentaLinea> Add(PedVentaLinea c)
        {
            ResultAndError<PedVentaLinea> result = new ResultAndError<PedVentaLinea>();
            try
            {
                db.PedVentaLineas.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<PedVentaLinea> Update(PedVentaLinea cr, bool insertIfNoExists = false)
        {
            ResultAndError<PedVentaLinea> result = new ResultAndError<PedVentaLinea>();
            try
            {
                PedVentaLinea old = Get(cr.DocumentNo, cr.LineNo);
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

        public ResultAndError<bool> UpdateReadingDate(IEnumerable<KeyValuePair<string, int>> pedventa, DateTimeOffset readingDate)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                foreach (KeyValuePair<string, int> no in pedventa)
                {
                    PedVentaLinea oPedVentaLineas = Get(no.Key, no.Value);
                    oPedVentaLineas.ReadingDate = readingDate;
                    db.SaveChanges();
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<PedVentaLinea> oPedVentaLineas, bool insertIfNoExists = false)
        {
            foreach (PedVentaLinea albaran in oPedVentaLineas)
            {
                PedVentaLinea old = Get(albaran.DocumentNo, albaran.LineNo);
                if (old == null && insertIfNoExists) Add(albaran);
                else db.Entry(old).CurrentValues.SetValues(albaran);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string documentNo, int lineNo)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                PedVentaLinea PedVentaLineas = Get(documentNo, lineNo);
                if (PedVentaLineas == null) return result.AddError("No se encontro el pedido venta linea con el documento " + documentNo);
                db.PedVentaLineas.Remove(PedVentaLineas);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<KeyValuePair<string, int>> PedVentaLineas)
        {
            List<PedVentaLinea> oAlbaran = new List<PedVentaLinea>();
            foreach (KeyValuePair<string, int> PedVentaLineasNo in PedVentaLineas)
            {
                PedVentaLinea oPedVentaLineas = Get(PedVentaLineasNo.Key, PedVentaLineasNo.Value);
                if (oPedVentaLineas != null) oAlbaran.Add(oPedVentaLineas);
            }
            db.PedVentaLineas.RemoveRange(oAlbaran);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
