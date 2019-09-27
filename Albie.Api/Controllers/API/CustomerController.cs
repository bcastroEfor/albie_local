using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Route("api/customer/[action]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ICustomerBS cBS;

        public CustomerController(ICustomerBS customer, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            cBS = customer;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListClients([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(cBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetClientById([FromQuery]string id)
        {
            return Ok(cBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdCustomer([FromBody]Customer customer, bool insertIfNoExists = false)
        {
            return Ok(cBS.Update(customer, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdCustomerMulti([FromBody]IEnumerable<Customer> customers, bool insertIfNoExists = false)
        {
            return Ok(cBS.UpdateMulti(customers, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelCustomer([FromQuery]string id)
        {
            return Ok(cBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelCustomerMulti([FromBody]IEnumerable<string> customers)
        {
            return Ok(cBS.DeleteMulti(customers));
        }
        #endregion
    }
}