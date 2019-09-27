using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Api.ViewModels;
using Albie.BS;
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
    [Route("api/albaranCompra/[action]")]
    [ApiController]
    public class AlbaranCompraController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IAlbaranCompraBS aBS;

        public AlbaranCompraController(IAlbaranCompraBS albaran, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            aBS = albaran;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListAlbaranCompras([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = aBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<AlbaranCompra_View> lista = new CollectionList<AlbaranCompra_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new AlbaranCompra_View(o))
            };
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult GetCollectionListAlbaranComprasReadingDate([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "rd")]DateTimeOffset readingDate, [FromQuery(Name = "rdf")]string readingDateFilter)
        {
            var result = aBS.GetCollectionListReadingDate(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, readingDate: readingDate, filterReadingDate: readingDateFilter);
            CollectionList<AlbaranCompra_View> lista = new CollectionList<AlbaranCompra_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new AlbaranCompra_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetAlbaranCompraById([FromQuery]string no)
        {
            return Ok(aBS.Get(no));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdAlbaranCompra([FromBody]AlbaranCompra AlbaranCompra, bool insertIfNoExists = false)
        {
            return Ok(aBS.Update(AlbaranCompra, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdAlbaranCompraMulti([FromBody]IEnumerable<AlbaranCompra> AlbaranCompras, bool insertIfNoExists = false)
        {
            return Ok(aBS.UpdateMulti(AlbaranCompras, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdAlbaranCompraReadingDate([FromBody]IEnumerable<string> ids, [FromQuery]DateTimeOffset dateReading)
        {
            return Ok(aBS.UpdateReadingDate(ids, dateReading));
        }

        [HttpDelete]
        public IActionResult DelAlbaranCompra([FromQuery]string no)
        {
            return Ok(aBS.Delete(no));
        }

        [HttpDelete]
        public IActionResult DelAlbaranCompraMulti([FromBody]IEnumerable<string> AlbaranCompra)
        {
            return Ok(aBS.DeleteMulti(AlbaranCompra));
        }

        [HttpPost]
        public IActionResult GenerateAlbaran([FromBody]Document order, [FromQuery(Name = "da")]DateTimeOffset fechaAlbaran, [FromQuery(Name = "nc")]bool nonConform = false)
        {
            aBS.RecepcionMercancia(order, fechaAlbaran, nonConform);
            return Ok();
        }

        [HttpPost]
        public IActionResult AnularAlbaran([FromBody]AlbaranCompra albaran)
        {
            aBS.AnularAlbaran(albaran);
            return Ok();
        }
        #endregion
    }
}