using ActioBP.General;
using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Api.ViewModels;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers
{
    [Route("api/order/[action]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IHeaderOrderBS hBS;

        public OrderController(IConfiguration conf, IHeaderOrderBS headerOrder) : base(conf)
        {
            _conf = conf;
            hBS = headerOrder;
        }

        #region GET
        [HttpGet]
        public IActionResult GetCabeceraById([FromQuery]int id)
        {
            return Ok(hBS.Get(id));
        }

        [HttpPost]
        public IActionResult GetCollectionListCabeceras([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = hBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<Document_View> lista = new CollectionList<Document_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new Document_View(o))
            };
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult GetCollectionListCabecerasReadingDate([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "rd")]DateTimeOffset readingDate, [FromQuery(Name = "rdf")]string readingDateFilter)
        {
            var result = hBS.GetCollectionListReadingDate(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, readingDate: readingDate, filterReadingDate: readingDateFilter);
            CollectionList<Document_View> lista = new CollectionList<Document_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new Document_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetEnumCodTypes()
        {
            IEnumerable<EnumMaster.Estado_Enum> all = EnumG.GetAll<EnumMaster.Estado_Enum>();
            return Ok(all.Select(o => new LabelAndValue<int>(o.GetDescEnum(), (int)o, o)));
        }
        #endregion

        #region POST
        public IActionResult CreateOrderByProvider([FromBody] CartList lista)
        {
            return Ok(hBS.CreateOrderByProvider(lista));
        }

        [HttpPost]
        public IActionResult UpdCabecera([FromBody]Document header, bool insertIfNoExists = false)
        {
            return Ok(hBS.Update(header, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdSalesCenterReadingDate([FromBody]IEnumerable<int> ids, [FromQuery]DateTimeOffset dateReading)
        {
            return Ok(hBS.UpdateReadingDate(ids, dateReading));
        }

        [HttpPost]
        public IActionResult UpdCabeceraMulti([FromBody]IEnumerable<Document> headers, bool insertIfNoExists = false)
        {
            return Ok(hBS.UpdateMulti(headers, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelCabecera([FromQuery]int id)
        {
            return Ok(hBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelCabeceraMulti([FromBody]IEnumerable<int> headers)
        {
            return Ok(hBS.DeleteMulti(headers));
        }
        #endregion
    }
}