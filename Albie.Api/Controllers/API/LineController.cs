using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Api.ViewModels;
using Albie.BS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers.API
{
    [Route("api/lineRequest/[action]")]
    [ApiController]
    public class LineController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ILineBS lBS;

        public LineController(IConfiguration conf, ILineBS line) : base(conf)
        {
            _conf = conf;
            lBS = line;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListLines([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = lBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<Lines_View> list = new CollectionList<Lines_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new Lines_View(o))
            };
            return Ok(list);
        }

        [HttpPost]
        public IActionResult GetCollectionListLinesReadingDate([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "rd")]DateTimeOffset readingDate, [FromQuery(Name = "rdf")]string readingDateFilter)
        {
            var result = lBS.GetCollectionListReadingDate(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, readingDate: readingDate, filterReadingDate: readingDateFilter);
            CollectionList<Lines_View> list = new CollectionList<Lines_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new Lines_View(o))
            };
            return Ok(list);
        }
        #endregion
    }
}