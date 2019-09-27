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
    public class InvoiceProviderDiscountBS : EntityBS, IInvoiceProviderDiscountBS
    {
        private readonly IConfiguration _conf;
        public InvoiceProviderDiscountBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<InvoiceProviderDiscount> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetInvoiceProviderDiscountCount(filter, filterArr);

            if (total == 0) return new CollectionList<InvoiceProviderDiscount>();

            var items = GetInvoiceProviderDiscountList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<InvoiceProviderDiscount>
            {
                Items = items,
                Total = total
            };
        }

        public int GetInvoiceProviderDiscountCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<InvoiceProviderDiscount> lista = db.InvoiceProviderDiscounts
                                           .WhereAct(filterArr, filter, fieldFilter: "no", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<InvoiceProviderDiscount> GetInvoiceProviderDiscountList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<InvoiceProviderDiscount> lista = db.InvoiceProviderDiscounts
                                           .WhereAct(filterArr, filter, fieldFilter: "No", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public InvoiceProviderDiscount Get(string id)
        {
            return db.InvoiceProviderDiscounts.SingleOrDefault(o => o.Code == id);
        }
        #endregion

        #region POST
        public ResultAndError<InvoiceProviderDiscount> Add(InvoiceProviderDiscount c)
        {
            ResultAndError<InvoiceProviderDiscount> result = new ResultAndError<InvoiceProviderDiscount>();
            try
            {
                db.InvoiceProviderDiscounts.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<InvoiceProviderDiscount> Update(InvoiceProviderDiscount c, bool insertIfNoExists = false)
        {
            ResultAndError<InvoiceProviderDiscount> result = new ResultAndError<InvoiceProviderDiscount>();
            try
            {
                InvoiceProviderDiscount old = Get(c.Code);
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

        public bool UpdateMulti(IEnumerable<InvoiceProviderDiscount> oInvoiceProviderDiscounts, bool insertIfNoExists = false)
        {
            foreach (InvoiceProviderDiscount invoiceProviderDiscount in oInvoiceProviderDiscounts)
            {
                InvoiceProviderDiscount old = Get(invoiceProviderDiscount.Code);
                if (old == null && insertIfNoExists) Add(invoiceProviderDiscount);
                else db.Entry(old).CurrentValues.SetValues(invoiceProviderDiscount);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                InvoiceProviderDiscount invoiceProviderDiscount = Get(id);
                if (invoiceProviderDiscount == null) return result.AddError("No se encontro descuento con el id " + id);
                db.InvoiceProviderDiscounts.Remove(invoiceProviderDiscount);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> InvoiceProviderDiscounts)
        {
            List<InvoiceProviderDiscount> oInvoiceProviderDiscounts = new List<InvoiceProviderDiscount>();
            foreach (string InvoiceProviderDiscountNo in InvoiceProviderDiscounts)
            {
                InvoiceProviderDiscount oInvoiceProviderDiscount = Get(InvoiceProviderDiscountNo);
                if (oInvoiceProviderDiscount != null) oInvoiceProviderDiscounts.Add(oInvoiceProviderDiscount);
            }
            db.InvoiceProviderDiscounts.RemoveRange(oInvoiceProviderDiscounts);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
