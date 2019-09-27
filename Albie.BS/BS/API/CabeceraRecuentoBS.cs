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
    public class CabeceraRecuentoBS : EntityBS, ICabeceraRecuentoBS
    {
        private readonly IConfiguration _conf;
        public CabeceraRecuentoBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<CabeceraRecuento> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            var total = GetCabeceraRecuentoCount(filter, filterArr);

            if (total == 0) return new CollectionList<CabeceraRecuento>();

            var items = GetCabeceraRecuentoList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<CabeceraRecuento>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCabeceraRecuentoCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<CabeceraRecuento> lista = db.CabeceraRecuentos
                                            .WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<CabeceraRecuento> GetCabeceraRecuentoList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<CabeceraRecuento> lista = db.CabeceraRecuentos                                           
                                           .WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public CabeceraRecuento Get(int id)
        {
            return db.CabeceraRecuentos.SingleOrDefault(o => o.IdRecuento == id);
        }
        #endregion

        #region POST
        public ResultAndError<CabeceraRecuento> Add(CabeceraRecuento pr)
        {
            ResultAndError<CabeceraRecuento> result = new ResultAndError<CabeceraRecuento>();
            try
            {
                db.CabeceraRecuentos.Add(pr);
                db.SaveChanges();
                return result.AddResult(pr);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<CabeceraRecuento> Update(CabeceraRecuento pr, bool insertIfNoExists = false)
        {
            ResultAndError<CabeceraRecuento> result = new ResultAndError<CabeceraRecuento>();
            try
            {
                CabeceraRecuento old = Get(pr.IdRecuento);
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
        #endregion
    }
}
