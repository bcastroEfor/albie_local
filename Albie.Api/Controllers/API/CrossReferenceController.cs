using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Route("api/crossReference/[action]")]
    [ApiController]
    public class CrossReferenceController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ICrossReferenceBS cBS;

        public CrossReferenceController(ICrossReferenceBS crossReference, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            cBS = crossReference;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListCrossReferences([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(cBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetCrossReferenceById([FromQuery]string id)
        {
            return Ok(cBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdCrossReference([FromBody]CrossReference crossReference, bool insertIfNoExists = false)
        {
            return Ok(cBS.Update(crossReference, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdCrossReferenceMulti([FromBody]IEnumerable<CrossReference> crossReferences, bool insertIfNoExists = false)
        {
            return Ok(cBS.UpdateMulti(crossReferences, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelCrossReference([FromQuery]string id)
        {
            return Ok(cBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelCrossReferenceMulti([FromBody]IEnumerable<string> crossReference)
        {
            return Ok(cBS.DeleteMulti(crossReference));
        }
        #endregion
    }
}