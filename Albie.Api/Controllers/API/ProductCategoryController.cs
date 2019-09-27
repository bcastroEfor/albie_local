using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Route("api/productCategory/[action]")]
    [ApiController]
    public class ProductCategoryController : BaseController
    {
        private readonly IConfiguration _conf;
        private readonly IProductCategoryBS pBS;

        public ProductCategoryController(IProductCategoryBS productCategory, IConfiguration conf, IHostingEnvironment _env) : base(conf)
        {
            _conf = conf;
            pBS = productCategory;
        }

        #region GET
        [HttpPost]
        public IActionResult GetCollectionListProductCategories([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(pBS.GetCollectionList(filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        [HttpGet]
        public IActionResult GetProductCategoryById([FromQuery]string id)
        {
            return Ok(pBS.Get(id));
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult UpdProductCategory([FromBody]ProductCategory productCategory, bool insertIfNoExists = false)
        {
            return Ok(pBS.Update(productCategory, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdProductCategoryMulti([FromBody]IEnumerable<ProductCategory> productCategories, bool insertIfNoExists = false)
        {
            return Ok(pBS.UpdateMulti(productCategories, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelProductCategory([FromQuery]string id)
        {
            return Ok(pBS.Delete(id));
        }

        [HttpDelete]
        public IActionResult DelProductCategoryMulti([FromBody]IEnumerable<string> productCategories)
        {
            return Ok(pBS.DeleteMulti(productCategories));
        }
        #endregion
    }
}