using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Route("api/customerCenter/[action]")]
    [ApiController]
    public class CustomerCenterController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ICustomerCenterBS cBS;

        public CustomerCenterController(ICustomerCenterBS CustomerCenter, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            cBS = CustomerCenter;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListCustomerCenters([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(cBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetCustomerCenterById([FromQuery]string id)
        {
            return Ok(cBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdCustomerCenter([FromBody]CustomerCenter customerCenter, bool insertIfNoExists = false)
        {
            return Ok(cBS.Update(customerCenter, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdCustomerCenterMulti([FromBody]IEnumerable<CustomerCenter> customerCenters, bool insertIfNoExists = false)
        {
            return Ok(cBS.UpdateMulti(customerCenters, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelCustomerCenter([FromQuery]string id)
        {
            return Ok(cBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelCustomerCenterMulti([FromBody]IEnumerable<string> customerCenter)
        {
            return Ok(cBS.DeleteMulti(customerCenter));
        }
        #endregion
    }
}