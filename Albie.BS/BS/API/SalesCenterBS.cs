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
    public class SalesCenterBS : EntityBS, ISalesCenterBS
    {
        private readonly IConfiguration _conf;
        private readonly IDateBS dBS;
        private readonly ICustomerRateBS cBS;
        public SalesCenterBS(RepoDB db, IDateBS date, ICustomerRateBS customerRate, IConfiguration conf) : base(db)
        {
            _conf = conf;
            dBS = date;
            cBS = customerRate;
        }

        #region GET
        public CollectionList<SalesCenter> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetSalesCenterCount(filter, filterArr);

            if (total == 0) return new CollectionList<SalesCenter>();

            var items = GetSalesCenterList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<SalesCenter>
            {
                Items = items,
                Total = total
            };
        }

        public CollectionList<SalesCenter> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {

            var total = GetSalesCenterCount(filter, filterArr, readingDate, filterReadingDate);

            if (total == 0) return new CollectionList<SalesCenter>();

            var items = GetSalesCenterList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, readingDate, filterReadingDate);

            return new CollectionList<SalesCenter>
            {
                Items = items,
                Total = total
            };
        }

        public int GetSalesCenterCount(string filter = "", List<FilterCriteria> filterArr = null, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<SalesCenter> lista = db.SalesCenters
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn);
            if (readingDate != null)
            {
                lista = FilterReadingDate(lista, readingDate, filterReadingDate);
                return lista.Count();
            }
            else lista = lista.Where(o => o.ReadingDate == null);
            return lista.Count();
        }

        public IEnumerable<SalesCenter> GetSalesCenterList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<SalesCenter> lista = db.SalesCenters
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IQueryable<SalesCenter> FilterReadingDate(IQueryable<SalesCenter> salesCenters, DateTimeOffset? readingDate, string readingDateFilter)
        {
            if (readingDateFilter == "<") return salesCenters = salesCenters.Where(o => o.ReadingDate < readingDate);
            else if (readingDateFilter == "<=") return salesCenters = salesCenters.Where(o => o.ReadingDate <= readingDate);
            else if (readingDateFilter == "=") return salesCenters = salesCenters.Where(o => o.ReadingDate == readingDate);
            else if (readingDateFilter == ">") return salesCenters = salesCenters.Where(o => o.ReadingDate > readingDate);
            else if (readingDateFilter == ">=") return salesCenters = salesCenters.Where(o => o.ReadingDate >= readingDate);
            return salesCenters = salesCenters.Where(o => o.ReadingDate != null);
        }

        public SalesCenter Get(string id)
        {
            return db.SalesCenters
                .Include(o => o.Customer)
                .Include(o => o.Center)
                .SingleOrDefault(o => o.EntryNo == Convert.ToInt32(id));
        }

        public VentasCentro<CustomerRate> GetVentasCentro(int year = -1, int month = -1, int mode = 7)
        {
            VentasCentro<CustomerRate> oVentas = new VentasCentro<CustomerRate>() { Items = cBS.GetCustomerRateList(pagesize: 0, sortName: "product.description") };
            if (mode == 1) oVentas.Dates = dBS.GetDaysInMonth(year, month);//daily
            else if (mode == 7) oVentas.Dates = dBS.GetWeeksInMonth(year, month).Select(o => new LabelAndValue<DateTime>(o.From.ToString("dd/MM") + " - " + o.To.ToString("dd/MM"), o.From, o.To));//weekly
            else if (mode == 12) oVentas.Dates = dBS.GetMonthsInYear().Select(o => new LabelAndValue<DateTime>(o.Value, new DateTime(year, o.Key, 1), o)); //monthly

            return FillSalesCenter(oVentas, mode);
        }

        public VentasCentro<CustomerRate> FillSalesCenter(VentasCentro<CustomerRate> ventasCentro, int mode = -1)
        {
            foreach (CustomerRate customer in ventasCentro.Items)
            {
                List<SalesCenter> oSales = new List<SalesCenter>();
                foreach (LabelAndValue<DateTime> date in ventasCentro.Dates)
                {
                    if (mode == 1)
                    {
                        SalesCenter sale = customer.SalesCenters.SingleOrDefault(o => o.PostingDate.Value.Day == date.Value.Day && o.PostingDate >= customer.StartingDate && (o.PostingDate <= customer.EndingDate || customer.EndingDate == null));
                        if (sale == null) oSales.Add(new SalesCenter());
                        else oSales.Add(sale);
                    }
                    else if (mode == 7)
                    {
                        SalesCenter weekSale = new SalesCenter() { Quantity = 0 };
                        foreach (SalesCenter sales in customer.SalesCenters)
                        {
                            if (sales.PostingDate.Value >= date.Value && sales.PostingDate.Value <= Convert.ToDateTime(date.Data)) weekSale.Quantity += sales.Quantity;
                        }
                        oSales.Add(weekSale);
                    }
                    else if (mode == 12)
                    {
                        SalesCenter monthSale = new SalesCenter() { Quantity = 0 };
                        foreach (SalesCenter sales in customer.SalesCenters)
                        {
                            if (sales.PostingDate.Value >= date.Value && sales.PostingDate.Value <= new DateTime(date.Value.Year, date.Value.Month, DateTime.DaysInMonth(date.Value.Year, date.Value.Month))) monthSale.Quantity += sales.Quantity;
                        }
                        oSales.Add(monthSale);
                    }
                }
                customer.SalesCenters = oSales;
            }

            return ventasCentro;
        }
        #endregion

        #region POST
        public ResultAndError<SalesCenter> Add(SalesCenter c)
        {
            ResultAndError<SalesCenter> result = new ResultAndError<SalesCenter>();
            try
            {
                db.SalesCenters.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<SalesCenter> Update(SalesCenter cr, bool insertIfNoExists = false)
        {
            ResultAndError<SalesCenter> result = new ResultAndError<SalesCenter>();
            try
            {
                SalesCenter old = Get(cr.ItemNo);
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

        public ResultAndError<bool> UpdateReadingDate(IEnumerable<int> centersNo, DateTimeOffset readingDate)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                foreach (int no in centersNo)
                {
                    SalesCenter oSalesCenter = Get(no.ToString());
                    oSalesCenter.ReadingDate = readingDate;
                    db.SaveChanges();
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<SalesCenter> oSalesCenters, bool insertIfNoExists = false)
        {
            foreach (SalesCenter SalesCenter in oSalesCenters)
            {
                SalesCenter old = Get(SalesCenter.ItemNo);
                if (old == null && insertIfNoExists) Add(SalesCenter);
                else db.Entry(old).CurrentValues.SetValues(SalesCenter);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                SalesCenter SalesCenter = Get(id);
                if (SalesCenter == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.SalesCenters.Remove(SalesCenter);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> SalesCenters)
        {
            List<SalesCenter> oSalesCenters = new List<SalesCenter>();
            foreach (string SalesCenterNo in SalesCenters)
            {
                SalesCenter oSalesCenter = Get(SalesCenterNo);
                if (oSalesCenter != null) oSalesCenters.Add(oSalesCenter);
            }
            db.SalesCenters.RemoveRange(oSalesCenters);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
