using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers.API
{
    [Route("api/familyProvider/[action]")]
    [ApiController]
    public class FamilyProviderController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IFamilyProviderBS fBS;

        public FamilyProviderController(IFamilyProviderBS family, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            fBS = family;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListFamilyProviders([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(fBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetFamilyProviderById([FromQuery]string id)
        {
            return Ok(fBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdFamilyProvider([FromBody]FamilyProvider familyProvider, bool insertIfNoExists = false)
        {
            return Ok(fBS.Update(familyProvider, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdFamilyProviderMulti([FromBody]IEnumerable<FamilyProvider> familyProviders, bool insertIfNoExists = false)
        {
            return Ok(fBS.UpdateMulti(familyProviders, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelFamilyProvider([FromQuery]string id)
        {
            return Ok(fBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelFamilyProviderMulti([FromBody]IEnumerable<string> familyProviders)
        {
            return Ok(fBS.DeleteMulti(familyProviders));
        }
        #endregion
    }
}