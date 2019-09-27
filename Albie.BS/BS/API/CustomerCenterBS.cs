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
    public class CustomerCenterBS : EntityBS, ICustomerCenterBS
    {
        private readonly IConfiguration _conf;
        public CustomerCenterBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<CustomerCenter> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetCustomerCenterCount(filter, filterArr);

            if (total == 0) return new CollectionList<CustomerCenter>();

            var items = GetCustomerCenterList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<CustomerCenter>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCustomerCenterCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<CustomerCenter> lista = db.CustomerCenters
                                           .WhereAct(filterArr, filter, fieldFilter: "no", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<CustomerCenter> GetCustomerCenterList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<CustomerCenter> lista = db.CustomerCenters
                                           .WhereAct(filterArr, filter, fieldFilter: "No", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public CustomerCenter Get(string id)
        {
            return db.CustomerCenters.Include(o => o.Center).Include(o => o.Customer).SingleOrDefault(o => o.CustomerNo == id);
        }
        #endregion

        #region POST
        public ResultAndError<CustomerCenter> Add(CustomerCenter c)
        {
            ResultAndError<CustomerCenter> result = new ResultAndError<CustomerCenter>();
            try
            {
                db.CustomerCenters.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<CustomerCenter> Update(CustomerCenter c, bool insertIfNoExists = false)
        {
            ResultAndError<CustomerCenter> result = new ResultAndError<CustomerCenter>();
            try
            {
                CustomerCenter old = Get(c.CustomerNo);
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

        public bool UpdateMulti(IEnumerable<CustomerCenter> oCustomerCenters, bool insertIfNoExists = false)
        {
            foreach (CustomerCenter CustomerCenter in oCustomerCenters)
            {
                CustomerCenter old = Get(CustomerCenter.CustomerNo);
                if (old == null && insertIfNoExists) Add(CustomerCenter);
                else db.Entry(old).CurrentValues.SetValues(CustomerCenter);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                CustomerCenter CustomerCenter = Get(id);
                if (CustomerCenter == null) return result.AddError("No se encontro el centro client con el id " + id);
                db.CustomerCenters.Remove(CustomerCenter);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> customerCenters)
        {
            List<CustomerCenter> oCustomerCenters = new List<CustomerCenter>();
            foreach (string customerCenterNo in customerCenters)
            {
                CustomerCenter oCustomerCenter = Get(customerCenterNo);
                if (oCustomerCenter != null) oCustomerCenters.Add(oCustomerCenter);
            }
            db.CustomerCenters.RemoveRange(oCustomerCenters);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
