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
    public class CustomerBS : EntityBS, ICustomerBS
    {
        private readonly IConfiguration _conf;
        public CustomerBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<Customer> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetClientCount(filter, filterArr);

            if (total == 0) return new CollectionList<Customer>();

            var items = GetClientList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<Customer>
            {
                Items = items,
                Total = total
            };
        }

        public int GetClientCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<Customer> lista = db.Customers
                                           .WhereAct(filterArr, filter, fieldFilter: "no", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<Customer> GetClientList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<Customer> lista = db.Customers
                                           .WhereAct(filterArr, filter, fieldFilter: "No", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public Customer Get(string id)
        {
            return db.Customers.Include(o => o.Centro).Include(o => o.Zona).SingleOrDefault(o => o.No == id);
        }
        #endregion

        #region POST
        public ResultAndError<Customer> Add(Customer c)
        {
            ResultAndError<Customer> result = new ResultAndError<Customer>();
            try
            {
                db.Customers.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Customer> Update(Customer c, bool insertIfNoExists = false)
        {
            ResultAndError<Customer> result = new ResultAndError<Customer>();
            try
            {
                Customer old = Get(c.No);
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

        public bool UpdateMulti(IEnumerable<Customer> oClients, bool insertIfNoExists = false)
        {
            foreach (Customer client in oClients)
            {
                Customer old = Get(client.No);
                if (old == null && insertIfNoExists) Add(client);
                else db.Entry(old).CurrentValues.SetValues(client);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                Customer client = Get(id);
                if (client == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.Customers.Remove(client);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> clients)
        {
            List<Customer> oClients = new List<Customer>();
            foreach (string clientNo in clients)
            {
                Customer oClient = Get(clientNo);
                if (oClient != null) oClients.Add(oClient);
            }
            db.Customers.RemoveRange(oClients);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
