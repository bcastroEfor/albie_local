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

namespace Albie.Api.Controllers
{
    [Route("api/salesCountedCenter/[action]")]
    [ApiController]
    public class SalesCountedCenterController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ISalesCountedCenterBS sBS;

        public SalesCountedCenterController(ISalesCountedCenterBS SalesCountedCenter, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            sBS = SalesCountedCenter;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListSalesCountedCenters([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = sBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<SalesCountedCenter_View> lista = new CollectionList<SalesCountedCenter_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new SalesCountedCenter_View(o))
            };
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult GetCollectionListSalesCountedCentersReadingDate([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "rd")]DateTimeOffset readingDate, [FromQuery(Name = "rdf")]string readingDateFilter)
        {
            var result = sBS.GetCollectionListReadingDate(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, readingDate: readingDate, filterReadingDate: readingDateFilter);
            CollectionList<SalesCountedCenter_View> lista = new CollectionList<SalesCountedCenter_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new SalesCountedCenter_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetSalesCountedCenterById([FromQuery]string id)
        {
            return Ok(sBS.Get(id));
        }

        [HttpGet]
        public IActionResult GetSalesCountedCenterList([FromQuery(Name = "y")]int year, [FromQuery(Name = "m")]int month, [FromQuery]int mode)
        {
            VentasCentro<Center> ventasCentro = sBS.GetVentasCentro(year: year, month: month, mode: mode);
            return Ok(new VentasContado_View(ventasCentro));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdSalesCountedCenter([FromBody]SalesCountedCenter SalesCountedCenter, bool insertIfNoExists = false)
        {
            return Ok(sBS.Update(SalesCountedCenter, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdSalesCountedCenterMulti([FromBody]IEnumerable<SalesCountedCenter> SalesCountedCenters, bool insertIfNoExists = false)
        {
            return Ok(sBS.UpdateMulti(SalesCountedCenters, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdSalesCenterReadingDate([FromBody]IEnumerable<int> ids, [FromQuery]DateTimeOffset dateReading)
        {
            return Ok(sBS.UpdateReadingDate(ids, dateReading));
        }

        [HttpDelete]
        public IActionResult DelSalesCountedCenter([FromQuery]string id)
        {
            return Ok(sBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelSalesCountedCenterMulti([FromBody]IEnumerable<string> SalesCountedCenter)
        {
            return Ok(sBS.DeleteMulti(SalesCountedCenter));
        }
        #endregion
    }
}