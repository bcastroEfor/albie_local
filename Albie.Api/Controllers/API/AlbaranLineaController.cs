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
    [Route("api/albaranLinea/[action]")]
    [ApiController]
    public class AlbaranLineaController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IAlbaranLineaBS aBS;

        public AlbaranLineaController(IAlbaranLineaBS albaran, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            aBS = albaran;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListAlbaranLineas([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = aBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<AlbaranLinea_View> lista = new CollectionList<AlbaranLinea_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new AlbaranLinea_View(o))
            };
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult GetCollectionListAlbaranLineasReadingDate([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "rd")]DateTimeOffset readingDate, [FromQuery(Name = "rdf")]string readingDateFilter)
        {
            var result = aBS.GetCollectionListReadingDate(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, readingDate: readingDate, filterReadingDate: readingDateFilter);
            CollectionList<AlbaranLinea_View> lista = new CollectionList<AlbaranLinea_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new AlbaranLinea_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetAlbaranLineaById([FromQuery]int no, [FromQuery]string albaranNo)
        {
            return Ok(aBS.Get(no, albaranNo));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdAlbaranLinea([FromBody]AlbaranLinea AlbaranLinea, bool insertIfNoExists = false)
        {
            return Ok(aBS.Update(AlbaranLinea, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdAlbaranLineaMulti([FromBody]IEnumerable<AlbaranLinea> AlbaranLineas, bool insertIfNoExists = false)
        {
            return Ok(aBS.UpdateMulti(AlbaranLineas, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdAlbaranLineaReadingDate([FromBody]IEnumerable<KeyValuePair<string, int>> ids, [FromQuery]DateTimeOffset dateReading)
        {
            return Ok(aBS.UpdateReadingDate(ids, dateReading));
        }

        [HttpDelete]
        public IActionResult DelAlbaranLinea([FromQuery]int no, [FromQuery]string albaranNo)
        {
            return Ok(aBS.Delete(no, albaranNo));
        }

        [HttpDelete]
        public IActionResult DelAlbaranLineaMulti([FromBody]IEnumerable<KeyValuePair<string, int>> AlbaranLinea)
        {
            return Ok(aBS.DeleteMulti(AlbaranLinea));
        }
        #endregion
    }
}