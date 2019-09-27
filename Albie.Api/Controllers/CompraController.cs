using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers
{
    //[Authorize]
    [Route("api/compra/[action]")]
    public class CompraController : BaseController
    {
        private IProductBS pBS;
        private IProviderBS prBS;
        private IListBS lBS;
        private IConfiguration _conf;

        public CompraController(IProductBS product, IProviderBS provider, IListBS list, IConfiguration configuration, IHostingEnvironment env) : base(configuration)
        {
            pBS = product;
            prBS = provider;
            lBS = list;
            _conf = configuration;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListProductsCategory([FromQuery(Name = "ParentCategory")]string parentCategory, [FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(pBS.GetProductCategoryCollectionList(productCategory: parentCategory, filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, hasUnitMeasureProductIncluded: true, hasProviderIncluded: true));
        }

        [HttpPost]
        public IActionResult GetCollectionListProducts([FromQuery(Name = "ParentCategory")]string parentCategory, [FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(pBS.GetCollectionListProducts(productCategory: parentCategory, filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, hasUnitMeasureProductIncluded: true));
        }
        
        #endregion

        #region POST
        [HttpPost]
        public IActionResult PostList([FromBody]List<Product> products, [FromQuery(Name = "nameList")]string nameList)
        {
            return Ok(lBS.PostList(products, nameList));
        }
        #endregion
    }
}