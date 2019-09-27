using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Api.ViewModels;
using Albie.BS;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers.API
{
    [Route("api/historicoPedido/[action]")]
    [ApiController]
    public class HistoricoPedidoController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IHistoricoPedidoBS hBS;

        public HistoricoPedidoController(IHistoricoPedidoBS historicoPedido, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            hBS = historicoPedido;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListHistoricoPedidos([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            var result = hBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<HistoricoPedido_View> lista = new CollectionList<HistoricoPedido_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new HistoricoPedido_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetHistoricoPedidoById([FromQuery]int no)
        {
            return Ok(hBS.Get(no));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdHistoricoPedido([FromBody]HistoricoPedido HistoricoPedido, bool insertIfNoExists = false)
        {
            return Ok(hBS.Update(HistoricoPedido, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdHistoricoPedidoMulti([FromBody]IEnumerable<HistoricoPedido> HistoricoPedidos, bool insertIfNoExists = false)
        {
            return Ok(hBS.UpdateMulti(HistoricoPedidos, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelHistoricoPedido([FromQuery]int no)
        {
            return Ok(hBS.Delete(no));
        }

        [HttpDelete]
        public IActionResult DelHistoricoPedidoMulti([FromBody]IEnumerable<int> HistoricoPedido)
        {
            return Ok(hBS.DeleteMulti(HistoricoPedido));
        }

        [HttpPost]
        public IActionResult CloseOrder([FromBody]Document order)
        {
            var result = hBS.CloseOrder(order);
            return Ok(result);
        }
        #endregion
    }
}