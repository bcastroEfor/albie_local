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
using System.Globalization;
using System.Linq;
using System.Net;

namespace Albie.BS
{
    public class CustomerRateBS : EntityBS, ICustomerRateBS
    {
        private readonly IConfiguration _conf;
        public CustomerRateBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<CustomerRate> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetCustomerRateCount(filter, filterArr);

            if (total == 0) return new CollectionList<CustomerRate>();

            var items = GetCustomerRateList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<CustomerRate>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCustomerRateCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<CustomerRate> lista = db.CustomerRates
                                           .WhereAct(filterArr, filter, fieldFilter: "no", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<CustomerRate> GetCustomerRateList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<CustomerRate> lista = db.CustomerRates
                                           .Include(o => o.Product)
                                           .Include(o => o.SalesCenters)
                                           .WhereAct(filterArr, filter, fieldFilter: "product.description", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public CustomerRate Get(string id)
        {
            return db.CustomerRates
                .Include(o => o.UnitMeasureProduct)
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .SingleOrDefault(o => o.ItemNo == id);
        }
        #endregion

        #region POST
        public ResultAndError<CustomerRate> Add(CustomerRate c)
        {
            ResultAndError<CustomerRate> result = new ResultAndError<CustomerRate>();
            try
            {
                db.CustomerRates.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<CustomerRate> Update(CustomerRate cr, bool insertIfNoExists = false)
        {
            ResultAndError<CustomerRate> result = new ResultAndError<CustomerRate>();
            try
            {
                CustomerRate old = Get(cr.ItemNo);
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

        public bool UpdateMulti(IEnumerable<CustomerRate> oCustomerRates, bool insertIfNoExists = false)
        {
            foreach (CustomerRate CustomerRate in oCustomerRates)
            {
                CustomerRate old = Get(CustomerRate.ItemNo);
                if (old == null && insertIfNoExists) Add(CustomerRate);
                else db.Entry(old).CurrentValues.SetValues(CustomerRate);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                CustomerRate CustomerRate = Get(id);
                if (CustomerRate == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.CustomerRates.Remove(CustomerRate);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> CustomerRates)
        {
            List<CustomerRate> oCustomerRates = new List<CustomerRate>();
            foreach (string CustomerRateNo in CustomerRates)
            {
                CustomerRate oCustomerRate = Get(CustomerRateNo);
                if (oCustomerRate != null) oCustomerRates.Add(oCustomerRate);
            }
            db.CustomerRates.RemoveRange(oCustomerRates);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
