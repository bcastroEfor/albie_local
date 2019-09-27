using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;
using Albie.Api.ViewModels;
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
    [Route("api/hojaRecuento/[action]")]
    [ApiController]
    public class HojaRecuentoController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IHojaRecuentoBS hBS;

        public HojaRecuentoController(IHojaRecuentoBS albaran, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            hBS = albaran;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListHojaRecuentos([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery(Name = "idR")]string idRecuento)
        {
            var result = hBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, idRecuento: idRecuento);
            CollectionList<HojaRecuento_View> lista = new CollectionList<HojaRecuento_View>()
            {
                Total = result.Total,
                Items = result.Items.Select(o => new HojaRecuento_View(o))
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetHojaRecuentoById([FromQuery]int no)
        {
            return Ok(hBS.Get(no));
        }

        [HttpGet]
        public IActionResult GetRecuento([FromQuery]string center, [FromQuery]string almacen, [FromQuery]string product, [FromQuery]string zona)
        {
            var items = hBS.IniciarRecuento(center, almacen, product, zona);
            return Ok(items);
        }

        [HttpGet]
        public IActionResult GetHojasByCodigo()
        {
            var lista = hBS.GetHojaRecuentoByCodigo();
            return Ok(lista);
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdHojaRecuento([FromBody]HojaRecuento HojaRecuento, bool insertIfNoExists = false)
        {
            return Ok(hBS.Update(HojaRecuento, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdHojaRecuentoMulti([FromBody]IEnumerable<HojaRecuento> HojaRecuentos, bool insertIfNoExists = false)
        {
            return Ok(hBS.UpdateMulti(HojaRecuentos, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelHojaRecuento([FromQuery]int no)
        {
            return Ok(hBS.Delete(no));
        }

        [HttpDelete]
        public IActionResult DelHojaRecuentoMulti([FromBody]IEnumerable<int> HojaRecuento)
        {
            return Ok(hBS.DeleteMulti(HojaRecuento));
        }
        #endregion
    }
}