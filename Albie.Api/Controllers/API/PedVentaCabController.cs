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
    [Route("api/pedVentaCab/[action]")]
    [ApiController]
    public class PedVentaCabController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IPedVentaCabBS pBS;

        public PedVentaCabController(IPedVentaCabBS pedCompra, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            pBS = pedCompra;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListPedVentaCabs([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = pBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<PedVentaCab_View> lista = new CollectionList<PedVentaCab_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new PedVentaCab_View(o))
            };
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult GetCollectionListPedVentaCabsReadingDate([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "rd")]DateTimeOffset readingDate, [FromQuery(Name = "rdf")]string readingDateFilter)
        {
            var result = pBS.GetCollectionListReadingDate(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, readingDate: readingDate, filterReadingDate: readingDateFilter);
            CollectionList<PedVentaCab_View> lista = new CollectionList<PedVentaCab_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new PedVentaCab_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetPedVentaCabById([FromQuery]string no)
        {
            return Ok(pBS.Get(no));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdPedVentaCab([FromBody]PedVentaCab PedVentaCab, bool insertIfNoExists = false)
        {
            return Ok(pBS.Update(PedVentaCab, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdPedVentaCabMulti([FromBody]IEnumerable<PedVentaCab> PedVentaCabs, bool insertIfNoExists = false)
        {
            return Ok(pBS.UpdateMulti(PedVentaCabs, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdPedVentaCabReadingDate([FromBody]IEnumerable<string> ids, [FromQuery]DateTimeOffset dateReading)
        {
            return Ok(pBS.UpdateReadingDate(ids, dateReading));
        }

        [HttpDelete]
        public IActionResult DelPedVentaCab([FromQuery]string no)
        {
            return Ok(pBS.Delete(no));
        }

        [HttpDelete]
        public IActionResult DelPedVentaCabMulti([FromBody]IEnumerable<string> PedVentaCab)
        {
            return Ok(pBS.DeleteMulti(PedVentaCab));
        }
        #endregion
    }
}