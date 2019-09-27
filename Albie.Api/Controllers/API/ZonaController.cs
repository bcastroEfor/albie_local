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
    [Route("api/zona/[action]")]
    [ApiController]
    public class ZonaController : BaseController
    {
        private IZonaBS iBS;
        private IConfiguration _conf;

        public ZonaController(IZonaBS zona, IConfiguration conf, IHostingEnvironment env) : base(conf)
        {
            iBS = zona;
            _conf = conf;
        }

        [HttpGet]
        public IActionResult GetZonaSelectMulti([FromQuery(Name = "al")]string almacen)
        {
            IEnumerable<Zona> lista = iBS.GetZonaList(almacen: almacen, pagesize: 0);
            return Ok(lista.Select(o => new MatSelect<string>(o.Name, o.Code)));
        }

        [HttpGet]
        public IActionResult GetZonaSelect([FromQuery(Name = "al")]string almacen)
        {
            IEnumerable<Zona> lista = iBS.GetZonaList(almacen: almacen, pagesize: 0);
            return Ok(lista.Select(o => new LabelAndValue<string>(o.Name, o.Code, o)));
        }
    }
}