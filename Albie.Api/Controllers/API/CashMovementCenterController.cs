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
    [Route("api/cashMovementCenter/[action]")]
    [ApiController]
    public class CashMovementCenterController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ICashMovementCenterBS cBS;

        public CashMovementCenterController(ICashMovementCenterBS CashMovementCenter, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            cBS = CashMovementCenter;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListCashMovementCenters([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = cBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<CashMovementCenter_View> lista = new CollectionList<CashMovementCenter_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new CashMovementCenter_View(o))
            };
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult GetCollectionListCashMovementCenterReadingDate([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "rd")]DateTimeOffset readingDate, [FromQuery(Name = "rdf")]string readingDateFilter)
        {
            var result = cBS.GetCollectionListReadingDate(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, readingDate: readingDate, filterReadingDate: readingDateFilter);
            CollectionList<CashMovementCenter_View> lista = new CollectionList<CashMovementCenter_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new CashMovementCenter_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetCashMovementCenterById([FromQuery]string id)
        {
            return Ok(cBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdCashMovementCenter([FromBody]CashMovementCenter CashMovementCenter, bool insertIfNoExists = false)
        {
            return Ok(cBS.Update(CashMovementCenter, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdCashMovementCenterMulti([FromBody]IEnumerable<CashMovementCenter> CashMovementCenters, bool insertIfNoExists = false)
        {
            return Ok(cBS.UpdateMulti(CashMovementCenters, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdCashMovementCenterReadingDate([FromBody]IEnumerable<int> ids, [FromQuery]DateTimeOffset dateReading)
        {
            return Ok(cBS.UpdateReadingDate(ids, dateReading));
        }

        [HttpDelete]
        public IActionResult DelCashMovementCenter([FromQuery]string id)
        {
            return Ok(cBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelCashMovementCenterMulti([FromBody]IEnumerable<string> CashMovementCenter)
        {
            return Ok(cBS.DeleteMulti(CashMovementCenter));
        }
        #endregion
    }
}