using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Api.ViewModels;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers.API
{
    [Route("api/pedVentaLinea/[action]")]
    [ApiController]
    public class PedVentaLineaController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IPedVentaLineaBS pBS;

        public PedVentaLineaController(IPedVentaLineaBS pedCompra, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            pBS = pedCompra;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListPedVentaLineas([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = pBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<PedVentaLinea_View> lista = new CollectionList<PedVentaLinea_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new PedVentaLinea_View(o))
            };
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult GetCollectionListPedVentaLineasReadingDate([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "rd")]DateTimeOffset readingDate, [FromQuery(Name = "rdf")]string readingDateFilter)
        {
            var result = pBS.GetCollectionListReadingDate(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, readingDate: readingDate, filterReadingDate: readingDateFilter);
            CollectionList<PedVentaLinea_View> lista = new CollectionList<PedVentaLinea_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new PedVentaLinea_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetPedVentaLineaById([FromQuery]string documentNo, [FromQuery]int lineNo)
        {
            return Ok(pBS.Get(documentNo, lineNo));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdPedVentaLinea([FromBody]PedVentaLinea PedVentaLinea, bool insertIfNoExists = false)
        {
            return Ok(pBS.Update(PedVentaLinea, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdPedVentaLineaMulti([FromBody]IEnumerable<PedVentaLinea> PedVentaLineas, bool insertIfNoExists = false)
        {
            return Ok(pBS.UpdateMulti(PedVentaLineas, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdPedVentaLineaReadingDate([FromBody]IEnumerable<KeyValuePair<string, int>> ids, [FromQuery]DateTimeOffset dateReading)
        {
            return Ok(pBS.UpdateReadingDate(ids, dateReading));
        }

        [HttpDelete]
        public IActionResult DelPedVentaLinea([FromQuery]string documentNo, [FromQuery]int lineNo)
        {
            return Ok(pBS.Delete(documentNo, lineNo));
        }

        [HttpDelete]
        public IActionResult DelPedVentaLineaMulti([FromBody]IEnumerable<KeyValuePair<string, int>> PedVentaLinea)
        {
            return Ok(pBS.DeleteMulti(PedVentaLinea));
        }
        #endregion
    }
}