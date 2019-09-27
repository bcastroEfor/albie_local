using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Api.ViewModels;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers.API
{
    [Route("api/customerRate/[action]")]
    [ApiController]
    public class CustomerRateController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly ICustomerRateBS cBS;

        public CustomerRateController(ICustomerRateBS CustomerRate, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            cBS = CustomerRate;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListCustomerRates([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            CollectionList<CustomerRate> lista = cBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<CustomerRate_View> result = new CollectionList<CustomerRate_View>()
            {
                Items = lista.Items.Select(o => new CustomerRate_View(o)),
                Total = lista.Total
            };
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetCustomerRateById([FromQuery]string id)
        {
            return Ok(cBS.Get(id));
        }               
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdCustomerRate([FromBody]CustomerRate customerRate, bool insertIfNoExists = false)
        {
            return Ok(cBS.Update(customerRate, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdCustomerRateMulti([FromBody]IEnumerable<CustomerRate> customerRates, bool insertIfNoExists = false)
        {
            return Ok(cBS.UpdateMulti(customerRates, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelCustomerRate([FromQuery]string id)
        {
            return Ok(cBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelCustomerRateMulti([FromBody]IEnumerable<string> customerRate)
        {
            return Ok(cBS.DeleteMulti(customerRate));
        }
        #endregion
    }
}