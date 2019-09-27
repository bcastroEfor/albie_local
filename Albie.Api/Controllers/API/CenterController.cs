using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Route("api/center/[action]")]
    [ApiController]
    public class CenterController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ICenterBS cBS;

        public CenterController(ICenterBS centro, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            cBS = centro;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListCentros([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(cBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetCentroById([FromQuery]string id)
        {
            return Ok(cBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdCentro([FromBody]Center centro, bool insertIfNoExists = false)
        {
            return Ok(cBS.Update(centro, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdCentroMulti([FromBody]IEnumerable<Center> centros, bool insertIfNoExists = false)
        {
            return Ok(cBS.UpdateMulti(centros, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelCentro([FromQuery]string id)
        {
            return Ok(cBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelCentroMulti([FromBody]IEnumerable<string> centros)
        {
            return Ok(cBS.DeleteMulti(centros));
        }
        #endregion
    }
}