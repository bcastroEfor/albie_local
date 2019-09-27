using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers.API
{
    [Route("api/invoiceProviderDiscount/[action]")]
    [ApiController]
    public class InvoiceProviderDiscountController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IInvoiceProviderDiscountBS iBS;

        public InvoiceProviderDiscountController(IInvoiceProviderDiscountBS invoice, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            iBS = invoice;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListInvoiceProviderDiscounts([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(iBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetInvoiceProviderDiscountById([FromQuery]string id)
        {
            return Ok(iBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdInvoiceProviderDiscount([FromBody]InvoiceProviderDiscount invoiceProviderDiscount, bool insertIfNoExists = false)
        {
            return Ok(iBS.Update(invoiceProviderDiscount, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdInvoiceProviderDiscountMulti([FromBody]IEnumerable<InvoiceProviderDiscount> invoiceProviderDiscounts, bool insertIfNoExists = false)
        {
            return Ok(iBS.UpdateMulti(invoiceProviderDiscounts, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelInvoiceProviderDiscount([FromQuery]string id)
        {
            return Ok(iBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelInvoiceProviderDiscountMulti([FromBody]IEnumerable<string> providers)
        {
            return Ok(iBS.DeleteMulti(providers));
        }
        #endregion
    }
}