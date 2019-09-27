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
    public class CenterBS : EntityBS, ICenterBS
    {
        private readonly IConfiguration _conf;
        public CenterBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public CollectionList<Center> GetCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {

            var total = GetCentroCount(filter, filterArr);

            if (total == 0) return new CollectionList<Center>();

            var items = GetCentroList(filter, filterArr, pageIndex, pagesize, sortName, sortDescending);

            return new CollectionList<Center>
            {
                Items = items,
                Total = total
            };
        }

        public int GetCentroCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<Center> lista = db.Centros
                                           .WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn);
            return lista.Count();
        }

        public IEnumerable<Center> GetCentroList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<Center> lista = db.Centros
                                           .Where(o => o.Code == "CEN_000011")//TODO Modificar por centro del usuario
                                           .Include(o => o.Subcenters).ThenInclude(s => s.SalesCountedCenters)
                                           .WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn)
                                           .OrderByAct(sortName, sortDescending);
            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public Center Get(string id)
        {
            return db.Centros
                .Include(o => o.Centro)
                .Include(o => o.Zona)
                .Include(o => o.Location)
                .Include(o => o.DefaultCustomer)
                .Include(o => o.PrincipalCustomer)
                .SingleOrDefault(o => o.Code == id);
        }
        #endregion

        #region POST
        public ResultAndError<Center> Add(Center c)
        {
            ResultAndError<Center> result = new ResultAndError<Center>();
            try
            {
                db.Centros.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Center> Update(Center cr, bool insertIfNoExists = false)
        {
            ResultAndError<Center> result = new ResultAndError<Center>();
            try
            {
                Center old = Get(cr.Code);
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

        public bool UpdateMulti(IEnumerable<Center> oCentros, bool insertIfNoExists = false)
        {
            foreach (Center centro in oCentros)
            {
                Center old = Get(centro.Code);
                if (old == null && insertIfNoExists) Add(centro);
                else db.Entry(old).CurrentValues.SetValues(centro);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string id)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                Center Centro = Get(id);
                if (Centro == null) return result.AddError("No se encontro la tarifa con el id " + id);
                db.Centros.Remove(Centro);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteMulti(IEnumerable<string> Centros)
        {
            List<Center> oCentros = new List<Center>();
            foreach (string CentroNo in Centros)
            {
                Center oCentro = Get(CentroNo);
                if (oCentro != null) oCentros.Add(oCentro);
            }
            db.Centros.RemoveRange(oCentros);
            db.SaveChanges();
            return true;
        }

        #endregion
    }
}
