using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Route("api/unitMeasureProduct/[action]")]
    [ApiController]
    public class UnitMeasureProductController : BaseController
    {
        IConfiguration _conf;
        IUnitMeasureProductBS uBS;

        public UnitMeasureProductController(IUnitMeasureProductBS unit, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            uBS = unit;
            _conf = conf;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListUnitMeasureProduct([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(uBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetUnitMeasureProductById([FromQuery]string id)
        {
            return Ok(uBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdUnitMeasureProduct([FromBody]UnitMeasureProduct subCenter, bool insertIfNoExists = false)
        {
            return Ok(uBS.Update(subCenter, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdUnitMeasureProductMulti([FromBody]IEnumerable<UnitMeasureProduct> unit, bool insertIfNoExists = false)
        {
            return Ok(uBS.UpdateMulti(unit, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelUnitMeasureProduct([FromQuery]string id)
        {
            return Ok(uBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelUnitMeasureProductMulti([FromBody]IEnumerable<string> unit)
        {
            return Ok(uBS.DeleteMulti(unit));
        }
        #endregion
    }
}