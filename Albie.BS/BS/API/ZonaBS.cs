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
    public class ZonaBS : EntityBS, IZonaBS
    {
        private readonly IConfiguration _conf;
        public ZonaBS(RepoDB db, IConfiguration conf) : base(db)
        {
            _conf = conf;
        }

        #region GET
        public IEnumerable<Zona> GetZonaList(string almacen = "", string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pagesize = 10, string sortName = "", bool sortDescending = false)
        {
            IQueryable<Zona> lista = db.Zonas
                                            .Where(o => o.LocationCode == almacen)
                                            .WhereAct(filterArr, filter, fieldFilter: "Name", opFilter: FilterOperator.Cn)
                                            .OrderByAct("name", sortDescending);

            if (pagesize == 0) return lista.ToList();
            return lista.Skip(pageIndex * pagesize).Take(pagesize).ToList();
        }
        #endregion
    }
}
