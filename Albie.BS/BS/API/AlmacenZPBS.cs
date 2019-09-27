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
    public class AlmacenZPBS : EntityBS, IAlmacenZPBS
    {
        private readonly IConfiguration _conf;
        public AlmacenZPBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<AlmacenZP> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            throw new NotImplementedException();
        }

        public CollectionList<AlmacenZP> GetCollectionListAlmacenes(string almacen = "", string zona = "", string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            var total = GetAlmacenZPCount(filter, filterArr, almacen, zona);

            if (total == 0) return new CollectionList<AlmacenZP>();

            var items = GetAlmacenZPList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending, almacen, zona);

            return new CollectionList<AlmacenZP>
            {
                Items = items,
                Total = total
            };
        }

        public int GetAlmacenZPCount(string filter = "", List<FilterCriteria> filterArr = null, string almacen = "", string zona = "")
        {
            IQueryable<AlmacenZP> lista = db.AlmacenZPs
                                            .Where(o => o.LocationCode == almacen && o.Zona == zona)
                                            .WhereAct(filterArr, filter, fieldFilter: "ProductNo", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<AlmacenZP> GetAlmacenZPList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false, string almacen = "", string zona = "")
        {
            IQueryable<AlmacenZP> lista = db.AlmacenZPs
                                           .Include(o => o.Location)
                                           .Include(o => o.Product)
                                           .Include(o => o.Zonas)
                                           .Where(o => o.LocationCode == almacen && o.Zona == zona)
                                           .WhereAct(filterArr, filter, fieldFilter: "product.description", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public AlmacenZP Get(string productId, string location)
        {
            return db.AlmacenZPs.SingleOrDefault(o => o.ProductNo == productId && o.LocationCode == location);
        }

        public AlmacenZP Get(string id)
        {
            return Get("", "");
        }
        #endregion

        #region POST
        public ResultAndError<AlmacenZP> Add(AlmacenZP pr)
        {
            ResultAndError<AlmacenZP> result = new ResultAndError<AlmacenZP>();
            try
            {
                db.AlmacenZPs.Add(pr);
                db.SaveChanges();
                return result.AddResult(pr);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<AlmacenZP> Update(AlmacenZP pr, bool insertIfNoExists = false)
        {
            ResultAndError<AlmacenZP> result = new ResultAndError<AlmacenZP>();
            try
            {
                AlmacenZP old = Get(pr.ProductNo, pr.LocationCode);
                if (old == null && insertIfNoExists) return Add(pr);
                db.Entry(old).CurrentValues.SetValues(pr);
                db.SaveChanges();
                return result.AddResult(pr);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool UpdateMulti(IEnumerable<AlmacenZP> oAlmacenZPs, bool insertIfNoExists = false)
        {
            foreach (AlmacenZP AlmacenZP in oAlmacenZPs)
            {
                AlmacenZP old = Get(AlmacenZP.ProductNo, AlmacenZP.LocationCode);
                if (old == null && insertIfNoExists) Add(AlmacenZP);
                else db.Entry(old).CurrentValues.SetValues(AlmacenZP);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            return Delete("", "");
        }

        public ResultAndError<bool> Delete(string productNo, string locationCode)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                AlmacenZP oAlmacenZP = Get(productNo, locationCode);
                if (oAlmacenZP == null) return result.AddError("No se encontro almacen con el id " + productNo + " " + locationCode);
                db.AlmacenZPs.Remove(oAlmacenZP);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> AlmacenZPs)
        {
            List<AlmacenZP> oAlmacenZPs = new List<AlmacenZP>();
            foreach (string AlmacenZPNo in AlmacenZPs)
            {
                AlmacenZP oAlmacenZP = Get(AlmacenZPNo);
                if (oAlmacenZP != null) oAlmacenZPs.Add(oAlmacenZP);
            }
            db.AlmacenZPs.RemoveRange(oAlmacenZPs);
            db.SaveChanges();
            return true;
        }

        public ResultAndError<AlmacenZP> AddProductToAlmacen(Product product, string almacen, string zona)
        {
            AlmacenZP oAlmacen = new AlmacenZP()
            {
                LocationCode = almacen,
                ProductNo = product.ProductNo,
                Zona = zona
            };
            return Add(oAlmacen);
        }

        
        #endregion
    }
}
