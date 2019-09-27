using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Albie.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albie.BS
{
    public class ListBS : EntityBS, IListBS
    {
        private IConfiguration _conf;
        private readonly ICartBS cBS;
        public ListBS(RepoDB db, IConfiguration conf, ICartBS cart) : base(db)
        {
            _conf = conf;
            cBS = cart;
        }

        #region GET
        public CollectionList<ProductList> GetCartProductListsCollectionList(Guid? listaId = null, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasProductIncluded = false, bool hasProviderRateIncluded = false)
        {

            var total = GetCartProductListsCount(filter, filterArr, listaId);

            if (total == 0) return new CollectionList<ProductList>();

            var items = GetCartProductListsList(listaId, filter, filterArr, pageIndex, pagesize, sortName, sortDescending, hasProductIncluded, hasProviderRateIncluded);

            return new CollectionList<ProductList>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCartProductListsCount(string filter = "", List<FilterCriteria> filterArr = null, Guid? listaId = null)
        {
            IQueryable<ProductList> lista = db.ProductLists.Where(o => o.ListId == listaId).WhereAct(filterArr, filter, fieldFilter: "product.description", opFilter: FilterOperator.Cn);

            return lista.Count();
        }

        public IEnumerable<ProductList> GetCartProductListsList(Guid? listaId = null, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, bool hasProductIncluded = false, bool hasProviderRateIncluded = false)
        {
            IQueryable<ProductList> lista = db.ProductLists
                                        .Where(o => o.ListId == listaId)
                                       .WhereAct(filterArr, filter, fieldFilter: "product.description", opFilter: FilterOperator.Cn)
                                       .OrderByAct(sortName, sortDescending);

            if (hasProductIncluded) lista = lista.Include(o => o.Product).ThenInclude(o => o.UnitMeasureProduct);
            if (hasProviderRateIncluded) lista = lista.Include(o => o.ProviderRate);

            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public IEnumerable<ProductList> GetProductListByListId(Guid listId, bool hasProductIncluded = false)
        {
            IQueryable<ProductList> lista = db.ProductLists.Where(o => o.ListId == listId);

            if (hasProductIncluded) lista = lista.Include(o => o.Product).ThenInclude(o => o.UnitMeasureProduct);

            return lista;
        }

        public ProductList GetProductListById(Guid id)
        {
            return db.ProductLists.Include(o => o.Product).Include(o => o.ProviderRate).SingleOrDefault(o => o.Id == id);
        }
        #endregion

        #region POST
        public List<ProductList> PostList(List<Product> products, string listName)
        {
            List<ProductList> oProductList = new List<ProductList>();

            CartList oList = cBS.CreateList(listName);
            foreach (Product product in products)
            {
                this.db.ProductLists.Add(new ProductList()
                {
                    ProductId = product.ProductNo,
                    Quantity = product.TotalUnits,
                    ListId = oList.Id,
                    ProviderRateId = product.ProviderRateId,
                    ProviderId = product.ProviderId
                });
            }
            this.db.SaveChanges();
            return oProductList;
        }

        public bool ChangeProductRate(ProviderRate rate, Guid productId)
        {
            //TO DO mirar como cambiar el id por los ids reales de productono y providerno
            ProductList oProduct = GetProductListById(productId);
            if (oProduct != null)
                oProduct.ProviderRateId = rate.Id;
            else
                return false;
            db.SaveChanges();
            return true;
        }
        #endregion
    }
}
