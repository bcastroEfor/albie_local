using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers
{
    [Route("api/location/[action]")]
    [ApiController]
    public class LocationController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ILocationBS lBS;

        public LocationController(ILocationBS Location, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            lBS = Location;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListLocations([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(lBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetLocationById([FromQuery]string id)
        {
            return Ok(lBS.Get(id));
        }

        [HttpGet]
        public IActionResult GetLocationSelect()
        {
            IEnumerable<Location> lista = lBS.GetWarehouseList(pagesize: 0);            
            return Ok(lista.Select(o => new LabelAndValue<string>(o.Name, o.Code, o)));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdLocation([FromBody]Location Locations, bool insertIfNoExists = false)
        {
            return Ok(lBS.Update(Locations, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdLocationMulti([FromBody]IEnumerable<Location> Locations, bool insertIfNoExists = false)
        {
            return Ok(lBS.UpdateMulti(Locations, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelLocation([FromQuery]string id)
        {
            return Ok(lBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelLocationMulti([FromBody]IEnumerable<string> Locations)
        {
            return Ok(lBS.DeleteMulti(Locations));
        }
        #endregion
    }
}