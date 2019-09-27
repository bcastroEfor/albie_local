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
    public class InvoiceLineDiscountBS : EntityBS, IInvoiceLineDiscountBS
    {
        private readonly IConfiguration _conf;
        public InvoiceLineDiscountBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<DiscountLineInvoice> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetDiscountLineInvoiceCount(filter, filterArr);

            if (total == 0) return new CollectionList<DiscountLineInvoice>();

            var items = GetDiscountLineInvoiceList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<DiscountLineInvoice>
            {
                Items = items,
                Total = total
            };
        }

        public int GetDiscountLineInvoiceCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<DiscountLineInvoice> lista = db.DiscountLineInvoices
                                           .WhereAct(filterArr, filter, fieldFilter: "no", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<DiscountLineInvoice> GetDiscountLineInvoiceList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<DiscountLineInvoice> lista = db.DiscountLineInvoices
                                           .WhereAct(filterArr, filter, fieldFilter: "No", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public DiscountLineInvoice Get(string id)
        {
            return db.DiscountLineInvoices.SingleOrDefault(o => o.ItemNo == id);
        }
        #endregion

        #region POST
        public ResultAndError<DiscountLineInvoice> Add(DiscountLineInvoice c)
        {
            ResultAndError<DiscountLineInvoice> result = new ResultAndError<DiscountLineInvoice>();
            try
            {
                db.DiscountLineInvoices.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<DiscountLineInvoice> Update(DiscountLineInvoice c, bool insertIfNoExists = false)
        {
            ResultAndError<DiscountLineInvoice> result = new ResultAndError<DiscountLineInvoice>();
            try
            {
                DiscountLineInvoice old = Get(c.ItemNo);
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

        public bool UpdateMulti(IEnumerable<DiscountLineInvoice> oDiscountLineInvoices, bool insertIfNoExists = false)
        {
            foreach (DiscountLineInvoice DiscountLineInvoice in oDiscountLineInvoices)
            {
                DiscountLineInvoice old = Get(DiscountLineInvoice.ItemNo);
                if (old == null && insertIfNoExists) Add(DiscountLineInvoice);
                else db.Entry(old).CurrentValues.SetValues(DiscountLineInvoice);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                DiscountLineInvoice DiscountLineInvoice = Get(id);
                if (DiscountLineInvoice == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.DiscountLineInvoices.Remove(DiscountLineInvoice);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> DiscountLineInvoices)
        {
            List<DiscountLineInvoice> oDiscountLineInvoices = new List<DiscountLineInvoice>();
            foreach (string DiscountLineInvoiceNo in DiscountLineInvoices)
            {
                DiscountLineInvoice oDiscountLineInvoice = Get(DiscountLineInvoiceNo);
                if (oDiscountLineInvoice != null) oDiscountLineInvoices.Add(oDiscountLineInvoice);
            }
            db.DiscountLineInvoices.RemoveRange(oDiscountLineInvoices);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
