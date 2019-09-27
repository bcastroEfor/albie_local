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
    public class SalesCountedCenterBS : EntityBS, ISalesCountedCenterBS
    {
        private readonly IConfiguration _conf;
        private readonly IDateBS dBS;
        private readonly ICenterBS cBS;
        public SalesCountedCenterBS(RepoDB db, IConfiguration conf, IDateBS date, ICenterBS center) : base(db)
        {
            _conf = conf;
            dBS = date;
            cBS = center;
        }

        #region GET
        public CollectionList<SalesCountedCenter> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetSalesCountedCenterCount(filter, filterArr);

            if (total == 0) return new CollectionList<SalesCountedCenter>();

            var items = GetSalesCountedCenterList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<SalesCountedCenter>
            {
                Items = items,
                Total = total
            };
        }

        public CollectionList<SalesCountedCenter> GetCollectionListReadingDate(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {

            var total = GetSalesCountedCenterCount(filter, filterArr, readingDate, filterReadingDate);

            if (total == 0) return new CollectionList<SalesCountedCenter>();

            var items = GetSalesCountedCenterList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, readingDate, filterReadingDate);

            return new CollectionList<SalesCountedCenter>
            {
                Items = items,
                Total = total
            };
        }

        public int GetSalesCountedCenterCount(string filter = "", List<FilterCriteria> filterArr = null, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<SalesCountedCenter> lista = db.SalesCountedCenters
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn);

            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);
            return lista.Count();
        }

        public IEnumerable<SalesCountedCenter> GetSalesCountedCenterList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, DateTimeOffset? readingDate = null, string filterReadingDate = "")
        {
            IQueryable<SalesCountedCenter> lista = db.SalesCountedCenters
                                           .WhereAct(filterArr, filter, fieldFilter: "centerCode", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (readingDate != null) lista = FilterReadingDate(lista, readingDate, filterReadingDate);
            else lista = lista.Where(o => o.ReadingDate == null);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IQueryable<SalesCountedCenter> FilterReadingDate(IQueryable<SalesCountedCenter> salesCountedCenters, DateTimeOffset? readingDate, string readingDateFilter)
        {
            if (readingDateFilter == "<") return salesCountedCenters = salesCountedCenters.Where(o => o.ReadingDate < readingDate);
            else if (readingDateFilter == "<=") return salesCountedCenters = salesCountedCenters.Where(o => o.ReadingDate <= readingDate);
            else if (readingDateFilter == "=") return salesCountedCenters = salesCountedCenters.Where(o => o.ReadingDate == readingDate);
            else if (readingDateFilter == ">") return salesCountedCenters = salesCountedCenters.Where(o => o.ReadingDate > readingDate);
            else if (readingDateFilter == ">=") return salesCountedCenters = salesCountedCenters.Where(o => o.ReadingDate >= readingDate);
            return salesCountedCenters = salesCountedCenters.Where(o => o.ReadingDate != null);
        }

        public SalesCountedCenter Get(string id)
        {
            return db.SalesCountedCenters.Include(o => o.Subcenter).Include(o => o.Center).SingleOrDefault(o => o.EntryNo == Convert.ToInt32(id));
        }

        public VentasCentro<Center> GetVentasCentro(int year = -1, int month = -1, int mode = 7)
        {
            VentasCentro<Center> oVentas = new VentasCentro<Center>() { Items = cBS.GetCentroList(pagesize: 0, sortName: "name") };
            if (mode == 1) oVentas.Dates = dBS.GetDaysInMonth(year, month);//daily
            else if (mode == 7) oVentas.Dates = dBS.GetWeeksInMonth(year, month).Select(o => new LabelAndValue<DateTime>(o.From.ToString("dd/MM") + " - " + o.To.ToString("dd/MM"), o.From, o.To));//weekly
            else if (mode == 12) oVentas.Dates = dBS.GetMonthsInYear().Select(o => new LabelAndValue<DateTime>(o.Value, new DateTime(year, o.Key, 1), o)); //monthly

            return FillSalesCenter(oVentas, mode);
        }

        public VentasCentro<Center> FillSalesCenter(VentasCentro<Center> ventasCentro, int mode = -1)
        {
            foreach (Center center in ventasCentro.Items)
            {
                foreach (Subcenter subcenter in center.Subcenters)
                {
                    List<SalesCountedCenter> oSales = new List<SalesCountedCenter>();
                    foreach (LabelAndValue<DateTime> date in ventasCentro.Dates)
                    {
                        if (mode == 1)
                        {
                            SalesCountedCenter sale = subcenter.SalesCountedCenters.SingleOrDefault(o => o.PostingDate.Value.Day == date.Value.Day);
                            if (sale == null) oSales.Add(new SalesCountedCenter());
                            else oSales.Add(sale);
                        }
                        else if (mode == 7)
                        {
                            SalesCountedCenter weekSale = new SalesCountedCenter() { Amount = 0 };
                            foreach (SalesCountedCenter sales in subcenter.SalesCountedCenters)
                            {
                                if (sales.PostingDate.Value >= date.Value && sales.PostingDate.Value <= Convert.ToDateTime(date.Data)) weekSale.Amount += sales.Amount;
                            }
                            oSales.Add(weekSale);
                        }
                        else if (mode == 12)
                        {
                            SalesCountedCenter monthSale = new SalesCountedCenter() { Amount = 0 };
                            foreach (SalesCountedCenter sales in subcenter.SalesCountedCenters)
                            {
                                if (sales.PostingDate.Value >= date.Value
                                    && sales.PostingDate.Value <= new DateTime(date.Value.Year, date.Value.Month, DateTime.DaysInMonth(date.Value.Year, date.Value.Month))) monthSale.Amount += sales.Amount;
                            }
                            oSales.Add(monthSale);
                        }
                    }
                    subcenter.SalesCountedCenters = oSales;
                }
            }

            return ventasCentro;
        }
        #endregion

        #region POST
        public ResultAndError<SalesCountedCenter> Add(SalesCountedCenter c)
        {
            ResultAndError<SalesCountedCenter> result = new ResultAndError<SalesCountedCenter>();
            try
            {
                db.SalesCountedCenters.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<SalesCountedCenter> Update(SalesCountedCenter cr, bool insertIfNoExists = false)
        {
            ResultAndError<SalesCountedCenter> result = new ResultAndError<SalesCountedCenter>();
            try
            {
                SalesCountedCenter old = Get(cr.EntryNo.ToString());
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
                    SalesCountedCenter oSalesCountedCenter = Get(no.ToString());
                    oSalesCountedCenter.ReadingDate = readingDate;
                    db.SaveChanges();
                }
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<SalesCountedCenter> oSalesCountedCenters, bool insertIfNoExists = false)
        {
            foreach (SalesCountedCenter SalesCountedCenter in oSalesCountedCenters)
            {
                SalesCountedCenter old = Get(SalesCountedCenter.EntryNo.ToString());
                if (old == null && insertIfNoExists) Add(SalesCountedCenter);
                else db.Entry(old).CurrentValues.SetValues(SalesCountedCenter);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                SalesCountedCenter SalesCountedCenter = Get(id);
                if (SalesCountedCenter == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.SalesCountedCenters.Remove(SalesCountedCenter);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> SalesCountedCenters)
        {
            List<SalesCountedCenter> oSalesCountedCenters = new List<SalesCountedCenter>();
            foreach (string SalesCountedCenterNo in SalesCountedCenters)
            {
                SalesCountedCenter oSalesCountedCenter = Get(SalesCountedCenterNo);
                if (oSalesCountedCenter != null) oSalesCountedCenters.Add(oSalesCountedCenter);
            }
            db.SalesCountedCenters.RemoveRange(oSalesCountedCenters);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
