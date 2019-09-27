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
    [Route("api/salesCenter/[action]")]
    [ApiController]
    public class SalesCenterController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ISalesCenterBS sBS;

        public SalesCenterController(ISalesCenterBS SalesCenter, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            sBS = SalesCenter;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListSalesCenters([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = sBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<SalesCenter_View> lista = new CollectionList<SalesCenter_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new SalesCenter_View(o))
            };
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult GetCollectionListSalesCentersReadingDate([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "rd")]DateTimeOffset readingDate, [FromQuery(Name = "rdf")]string readingDateFilter)
        {
            var result = sBS.GetCollectionListReadingDate(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, readingDate: readingDate, filterReadingDate: readingDateFilter);
            CollectionList<SalesCenter_View> lista = new CollectionList<SalesCenter_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new SalesCenter_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetSalesCenterById([FromQuery]string id)
        {
            return Ok(sBS.Get(id));
        }

        [HttpGet]
        public IActionResult GetSalesCenterList([FromQuery(Name = "y")]int year, [FromQuery(Name = "m")]int month, [FromQuery]int mode)
        {
            VentasCentro<CustomerRate> ventasCentro = sBS.GetVentasCentro(year: year, month: month, mode: mode);
            return Ok(new VentasCentro_View(ventasCentro));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdSalesCenter([FromBody]SalesCenter SalesCenter, bool insertIfNoExists = false)
        {
            return Ok(sBS.Update(SalesCenter, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdSalesCenterMulti([FromBody]IEnumerable<SalesCenter> SalesCenters, bool insertIfNoExists = false)
        {
            return Ok(sBS.UpdateMulti(SalesCenters, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdSalesCenterReadingDate([FromBody]IEnumerable<int> ids, [FromQuery]DateTimeOffset dateReading)
        {
            return Ok(sBS.UpdateReadingDate(ids, dateReading));
        }

        [HttpDelete]
        public IActionResult DelSalesCenter([FromQuery]string id)
        {
            return Ok(sBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelSalesCenterMulti([FromBody]IEnumerable<string> SalesCenter)
        {
            return Ok(sBS.DeleteMulti(SalesCenter));
        }
        #endregion
    }
}