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
    [Route("api/almacenZP/[action]")]
    [ApiController]
    public class AlmacenZPController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IAlmacenZPBS aBS;

        public AlmacenZPController(IAlmacenZPBS almacen, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            aBS = almacen;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListAlmacenZPs([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery]string almacen, [FromQuery]string zona)
        {
            var result = aBS.GetCollectionListAlmacenes(almacen: almacen, zona: zona, filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending);
            CollectionList<AlmacenZP_View> lista = new CollectionList<AlmacenZP_View>()
            {
                Items = result.Items.Select(o => new AlmacenZP_View(o)),
                Total = result.Total
            };
            return Ok(lista);
        }

        [HttpGet]
        public IActionResult GetAlmacenZPById([FromQuery]string no)
        {
            return Ok(aBS.Get(no));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdAlmacenZP([FromBody]AlmacenZP AlmacenZP, bool insertIfNoExists = false)
        {
            return Ok(aBS.Update(AlmacenZP, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdAlmacenZPMulti([FromBody]IEnumerable<AlmacenZP> AlmacenZPs, bool insertIfNoExists = false)
        {
            return Ok(aBS.UpdateMulti(AlmacenZPs, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelAlmacenZP([FromQuery]string no)
        {
            return Ok(aBS.Delete(no));
        }

        [HttpDelete]
        public IActionResult DelAlmacenZPMulti([FromBody]IEnumerable<string> AlmacenZP)
        {
            return Ok(aBS.DeleteMulti(AlmacenZP));
        }

        [HttpPost]
        public IActionResult AddProductToAlmacen([FromBody]Product product, [FromQuery]string almacen, [FromQuery]string zona)
        {
            return Ok(aBS.AddProductToAlmacen(product, almacen, zona));
        }
        #endregion
    }
}