using System;
using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Api.ViewModels;
using Albie.BS;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers.API
{
    [Route("api/cabeceraRecuento/[action]")]
    [ApiController]
    public class CabeceraRecuentoController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ICabeceraRecuentoBS cBS;

        public CabeceraRecuentoController(ICabeceraRecuentoBS cabecera, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            cBS = cabecera;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListCabeceraRecuentos([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = cBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<CabeceraRecuento_View> lista = new CollectionList<CabeceraRecuento_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new CabeceraRecuento_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetCabeceraRecuentoById([FromQuery]int no)
        {
            return Ok(cBS.Get(no));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdCabeceraRecuento([FromBody]CabeceraRecuento CabeceraRecuento, bool insertIfNoExists = false)
        {
            return Ok(cBS.Update(CabeceraRecuento, insertIfNoExists));
        }
        #endregion
    }
}