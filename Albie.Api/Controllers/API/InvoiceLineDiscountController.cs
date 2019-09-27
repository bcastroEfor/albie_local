using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Route("api/invoiceLineDiscount/[action]")]
    [ApiController]
    public class InvoiceLineDiscountController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IInvoiceLineDiscountBS iBS;

        public InvoiceLineDiscountController(IInvoiceLineDiscountBS DiscountLineInvoice, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            iBS = DiscountLineInvoice;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListDiscountLineInvoices([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(iBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetDiscountLineInvoiceById([FromQuery]string id)
        {
            return Ok(iBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdDiscountLineInvoice([FromBody]DiscountLineInvoice discountLineInvoice, bool insertIfNoExists = false)
        {
            return Ok(iBS.Update(discountLineInvoice, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdDiscountLineInvoiceMulti([FromBody]IEnumerable<DiscountLineInvoice> discountLineInvoices, bool insertIfNoExists = false)
        {
            return Ok(iBS.UpdateMulti(discountLineInvoices, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelDiscountLineInvoice([FromQuery]string id)
        {
            return Ok(iBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelDiscountLineInvoiceMulti([FromBody]IEnumerable<string> lineDiscounts)
        {
            return Ok(iBS.DeleteMulti(lineDiscounts));
        }
        #endregion
    }
}