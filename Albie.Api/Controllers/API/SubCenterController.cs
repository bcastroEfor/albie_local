using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Route("api/subcentre/[action]")]
    [ApiController]
    public class SubCenterController : BaseController
    {
        IConfiguration _conf;
        ISubCentreBS sBS;

        public SubCenterController(ISubCentreBS subcentre, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            sBS = subcentre;
            _conf = conf;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListSubCenters([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(sBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetSubCenterById([FromQuery]string id)
        {
            return Ok(sBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdSubCenter([FromBody]Subcenter subCenter, bool insertIfNoExists = false)
        {
            return Ok(sBS.Update(subCenter, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdSubCenterMulti([FromBody]IEnumerable<Subcenter> subCenters, bool insertIfNoExists = false)
        {
            return Ok(sBS.UpdateMulti(subCenters, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelSubCenter([FromQuery]string id)
        {
            return Ok(sBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelSubCenterMulti([FromBody]IEnumerable<string> subCenters)
        {
            return Ok(sBS.DeleteMulti(subCenters));
        }
        #endregion
    }
}