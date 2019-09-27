using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Route("api/cartlist/[action]")]
    public class CartListController : BaseController
    {
        private readonly IListBS lBS;
        private readonly ICartBS cBS;
        private readonly IConfiguration _conf;

        public CartListController(IListBS lista, ICartBS cart, IConfiguration conf, IHostingEnvironment env) : base(conf)
        {
            lBS = lista;
            cBS = cart;
            _conf = conf;
        }

        #region GET
        [HttpGet("{id}")]
        public IActionResult GetProductListByListId(Guid id)
        {
            return Ok(lBS.GetProductListByListId(id, hasProductIncluded: true));
        }

        [HttpPost]
        public IActionResult GetCollectionListCartList([FromQuery(Name = "PedidoHabitual")]bool pedidoHabitual, [FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(cBS.GetCartListsCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, hasListProductIncluded: true, pedidoHabitual: pedidoHabitual));
        }

        [HttpPost]
        public IActionResult GetCollectionListProductCartList([FromQuery(Name = "listaId")]Guid listaId, [FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(lBS.GetCartProductListsCollectionList(listaId: listaId, filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, hasProductIncluded: true, hasProviderRateIncluded: true));
        }
        #endregion

        #region POST
        [HttpPost("{id}")]
        public IActionResult SetAsUsualOrder(Guid id)
        {
            return Ok(cBS.SetAsUsualOrder(id));
        }
        #endregion
    }
}