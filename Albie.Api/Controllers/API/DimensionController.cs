using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Albie.Api.Controllers
{
    [Route("api/dimension/[action]")]
    [ApiController]
    public class DimensionController : BaseController
    {
        private IConfiguration _conf;
        private IDimensionBS dBS;

        public DimensionController(IConfiguration conf, IDimensionBS dimension) : base(conf)
        {
            _conf = conf;
            dBS = dimension;
        }

        #region GET
        [HttpGet]
        public IActionResult GetProviderSelect([FromQuery(Name = "dc")]string dimensionCode)
        {
            IEnumerable<Dimension> lista = dBS.GetDimensionList(pagesize: 0, dimensionCode: dimensionCode);
            return Ok(lista.Select(o => new LabelAndValue<string>(o.Name, o.Code, o)).OrderBy(o => o.Label));
        }
        #endregion

        [HttpGet]
        public IActionResult GetDimensionesSelectAutocomplete([FromQuery(Name = "f")]string filtro = "", [FromQuery(Name = "ps")]int pageSize = 10, [FromQuery(Name = "pi")]int pageIndex = 0, [FromQuery]string dimensionCode = "")
        {
            IEnumerable<Dimension> lista = dBS.GetDimensionList(filter: filtro, pagesize: pageSize, pageIndex: pageIndex, dimensionCode: dimensionCode);
            return Ok(lista.Select(e => new LabelAndValue<string>(e.Name, e.Code, e)));
        }

        #region POST
        [HttpPost]
        public IActionResult UpdDimension([FromBody]Dimension Dimension, bool insertIfNoExists = false)
        {
            return Ok(dBS.Update(Dimension, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdDimensionMulti([FromBody]IEnumerable<Dimension> Dimensions, bool insertIfNoExists = false)
        {
            return Ok(dBS.UpdateMulti(Dimensions, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelDimension([FromQuery]string code, [FromQuery]string dimensionCode)
        {
            return Ok(dBS.Delete(code, dimensionCode));
        }
        #endregion
    }
}