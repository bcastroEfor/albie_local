using ActioBP.General.Models;
using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Albie.Repository.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Albie.BS
{
    public class DimensionBS : EntityBS, IDimensionBS
    {
        private readonly IConfiguration _conf;
        public DimensionBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET

        /// <summary>
        /// Obtiene un listado de las dimensiones
        /// </summary>
        /// <param name="dimensionCode">Campo para filtrar las dimensiones Ej: Centro</param>
        /// <param name="filter"></param>
        /// <param name="filterArr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pagesize"></param>
        /// <param name="sortName"></param>
        /// <param name="sortDescending"></param>
        /// <returns>Ienumerable<Dimension></returns>
        public IEnumerable<Dimension> GetDimensionList(string dimensionCode = "", string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<Dimension> lista = db.Dimensions.WhereAct(filterArr, filter, fieldFilter: "Name", opFilter: FilterOperator.Cn)
                                                    .OrderByAct("Name", sortDescending);
            if (!string.IsNullOrEmpty(dimensionCode)) lista = lista.Where(o => o.DimensionCode == dimensionCode);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }

        public Dimension Get(string code, string dimensionCode)
        {
            return db.Dimensions.SingleOrDefault(o => o.Code == code && o.DimensionCode == dimensionCode);
        }
        #endregion

        #region POST
        public ResultAndError<Dimension> Add(Dimension c)
        {
            ResultAndError<Dimension> result = new ResultAndError<Dimension>();
            try
            {
                db.Dimensions.Add(c);
                db.SaveChanges();
                return result.AddResult(c);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        public ResultAndError<Dimension> Update(Dimension cr, bool insertIfNoExists = false)
        {
            ResultAndError<Dimension> result = new ResultAndError<Dimension>();
            try
            {
                Dimension old = Get(cr.Code, cr.DimensionCode);
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

        public bool UpdateMulti(IEnumerable<Dimension> oDimensions, bool insertIfNoExists = false)
        {
            foreach (Dimension dimension in oDimensions)
            {
                Dimension old = Get(dimension.Code, dimension.DimensionCode);
                if (old == null && insertIfNoExists) Add(dimension);
                else db.Entry(old).CurrentValues.SetValues(dimension);
            }
            db.SaveChanges();
            return true;
        }

        public ResultAndError<bool> Delete(string code, string dimensionCode)
        {
            ResultAndError<bool> result = new ResultAndError<bool>();
            try
            {
                Dimension Centro = Get(code, dimensionCode);
                if (Centro == null) return result.AddError("No se encontro la tarifa con el id " + code);
                db.Dimensions.Remove(Centro);
                db.SaveChanges();
                return result.AddResult(true);
            }
            catch (Exception e)
            {
                return result.AddError(e, HttpStatusCode.InternalServerError);
            }
        }

        #endregion
    }
}
