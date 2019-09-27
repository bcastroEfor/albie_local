using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers.API
{
    [Route("api/zoneProvider/[action]")]
    [ApiController]
    public class ZoneProviderController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IZoneProviderBS cBS;

        public ZoneProviderController(IZoneProviderBS ZoneProvider, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            cBS = ZoneProvider;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListZoneProviders([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(cBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetZoneProviderById([FromQuery]string id)
        {
            return Ok(cBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdZoneProvider([FromBody]ZoneProvider zoneProvider, bool insertIfNoExists = false)
        {
            return Ok(cBS.Update(zoneProvider, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdZoneProviderMulti([FromBody]IEnumerable<ZoneProvider> zoneProviders, bool insertIfNoExists = false)
        {
            return Ok(cBS.UpdateMulti(zoneProviders, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelZoneProvider([FromQuery]string id)
        {
            return Ok(cBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelZoneProviderMulti([FromBody]IEnumerable<string> zoneProviders)
        {
            return Ok(cBS.DeleteMulti(zoneProviders));
        }
        #endregion
    }
}