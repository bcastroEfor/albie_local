using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers
{
    [Route("api/provider/[action]")]
    [ApiController]
    public class ProviderController : BaseController
    {
        private IConfiguration _conf;
        private IProviderBS pBS;

        public ProviderController(IConfiguration conf, IProviderBS provider) : base(conf)
        {
            _conf = conf;
            pBS = provider;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListCentros([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(pBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetProviderById([FromQuery]string id)
        {
            return Ok(pBS.Get(id));
        }

        [HttpGet]
        public IActionResult GetProviderSelect([FromQuery(Name = "ParentCategory")]string parentCategory)
        {
            IEnumerable<Provider> lista = pBS.GetProviderList(pagesize: 0);
            return Ok(lista.Select(o => new LabelAndValue<string>(o.Name, o.VendorNo, o)).OrderBy(o => o.Label));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdProvider([FromBody]Provider oProvider, bool insertIfNoExists = false)
        {
            return Ok(pBS.Update(oProvider, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdCentroMulti([FromBody]IEnumerable<Provider> provider, bool insertIfNoExists = false)
        {
            return Ok(pBS.UpdateMulti(provider, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelProvider([FromQuery]string id)
        {
            return Ok(pBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelCentroMulti([FromBody]IEnumerable<string> providers)
        {
            return Ok(pBS.DeleteMulti(providers));
        }
        #endregion
    }
}