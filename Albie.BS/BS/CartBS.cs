using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Albie.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albie.BS
{
    public class CartBS : EntityBS, ICartBS
    {
        private IConfiguration _conf;
        public CartBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<CartList> GetCartListsCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasListProductIncluded = false, bool pedidoHabitual = false)
        {

            var total = GetCartListsCount(filter, filterArr, pedidoHabitual);

            if (total == 0) return new CollectionList<CartList>();

            var items = GetCartListsList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, hasListProductIncluded, pedidoHabitual);

            return new CollectionList<CartList>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCartListsCount(string filter = "", List<FilterCriteria> filterArr = null, bool pedidoHabitual = false)
        {
            IQueryable<CartList> lista = db.Lists.Where(o => o.PedidoHabitual == pedidoHabitual).WhereAct(filterArr, filter, fieldFilter: "nombre", opFilter: FilterOperator.Cn);

            return lista.Count();
        }

        public IEnumerable<CartList> GetCartListsList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasListProductIncluded = false, bool pedidoHabitual = false)
        {
            IQueryable<CartList> lista = db.Lists
                                       .Where(o => o.PedidoHabitual == pedidoHabitual)
                                       .WhereAct(filterArr, filter, fieldFilter: "nombre", opFilter: FilterOperator.Cn)
                                       .OrderByAct("nombre", sortDescending);

            if (hasListProductIncluded)
            {
                lista = lista.Include(o => o.ProductList).ThenInclude(o => o.Product).ThenInclude(o => o.UnitMeasureProduct);
                lista = lista.Include(o => o.ProductList).ThenInclude(o => o.Product).ThenInclude(o => o.ProviderRates);
                lista = lista.Include(o => o.ProductList).ThenInclude(o => o.Product).ThenInclude(o => o.DiscountLineInvoice);
            }

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public CartList GetListById(Guid id)
        {
            return db.Lists.Include(o => o.ProductList).SingleOrDefault(o => o.Id == id);
        }
        #endregion

        #region POST
        [HttpPost]
        public CartList CreateList(string name)
        {
            CartList oList = new CartList()
            {
                Nombre = name,
                F_Creacion = DateTime.Now,
                PedidoHabitual = false
            };
            this.db.Lists.Add(oList);
            this.db.SaveChanges();
            return oList;
        }

        public bool SetAsUsualOrder(Guid idLista)
        {
            CartList oList = GetListById(idLista);
            oList.PedidoHabitual = true;
            db.SaveChanges();
            return true;
        }
        #endregion
    }
}
