using ActioBP.Linq.FilterLinq;
using Albie.Api.ViewModels;
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
    [Route("api/product/[action]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private IProductBS pBS;
        private IConfiguration _conf;

        public ProductController(IProductBS product, IConfiguration conf, IHostingEnvironment env) : base(conf)
        {
            pBS = product;
            _conf = conf;
        }

        #region GET
        /// <summary>
        /// Metodo para obtener listado de productos por categoria
        /// </summary>
        /// <param name="parentCategory"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProductsList()
        {
            return Ok(pBS.GetProductsList(hasUnitMeasureProductIncluded: true, hasProductCategoryIncluded: true));
        }

        /// <summary>
        /// Obtiene un producto por su "ProductNo"
        /// </summary>
        /// <param name="productNo">Id del producto</param>
        /// <returns>Objeto producto</returns>
        [HttpGet]
        public IActionResult GetProductById([FromQuery]string productNo)
        {
            return Ok(pBS.Get(productNo));
        }

        /// <summary>
        /// Obtiene un dropdown de los productos
        /// </summary>
        /// <param name="parentCategory"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProductSelect([FromQuery(Name = "ParentCategory")]string parentCategory)
        {
            IEnumerable<ProductCategory> lista = pBS.GetProductCategoryList(parentCategory: parentCategory);
            return Ok(lista.Select(o => new LabelAndValue<string>(o.Description, o.Code, o)));
        }

        [HttpGet]
        public IActionResult GetProductosList([FromQuery(Name = "ParentCategory")]string parentCategory)
        {
            IEnumerable<Product> lista = pBS.GetProductList(productCategory: parentCategory, hasUnitMeasureProductIncluded: true, hasProductCategoryIncluded: true);
            return Ok(lista.Select(o => new Product_View(o)));
        }

        /// <summary>
        /// Listado de productos para un modulo de autocomplete
        /// </summary>
        /// <param name="filtro">filtro para buscar en los productos</param>
        /// <param name="pageSize">10</param>
        /// <param name="pageIndex">0</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProductosSelectAutocomplete([FromQuery(Name = "f")]string filtro = "", [FromQuery(Name = "ps")]int pageSize = 10, [FromQuery(Name = "pi")]int pageIndex = 0)
        {
            IEnumerable<Product> lista = pBS.GetProductList(filter: filtro, pagesize: pageSize, pageIndex: pageIndex, hasProductCategoryIncluded: true, hasUnitMeasureProductIncluded: true);
            return Ok(lista.Select(e => new LabelAndValue<string>(e.Description, e.ProductNo, e)).OrderBy(o => o.Label));
        }

        [HttpPost]
        public IActionResult GetCollectionListProducts([FromQuery(Name = "ParentCategory")]string parentCategory, [FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(pBS.GetCollectionListProducts(productCategory: parentCategory, filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending, hasUnitMeasureProductIncluded: true));
        }

        [HttpPost]
        public IActionResult GetCollectionListProductsAlmacen([FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending, [FromQuery]string filtro)
        {
            return Ok(pBS.GetCollectionList(filter: filtro, filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }
        #endregion

        #region POST
        /// <summary>
        /// Metodo para actualizar un producto, si no existe lo crea.
        /// </summary>
        /// <param name="product">Objeto de producto</param>
        /// <param name="insertIfNoExists">Por defecto "false"</param>
        /// <returns>bool</returns>
        [HttpPost]
        public IActionResult UpdProduct([FromBody]Product product, bool insertIfNoExists = false)
        {
            return Ok(pBS.Update(product, insertIfNoExists));
        }

        /// <summary>
        /// Metodo para actualizar varios productos, si no existen los crea.
        /// </summary>
        /// <param name="products">Listado de productos</param>
        /// <param name="insertIfNoExists"></param>
        /// <returns>bool</returns>
        [HttpPost]
        public IActionResult UpdProductMulti([FromBody]IEnumerable<Product> products, bool insertIfNoExists = false)
        {
            return Ok(pBS.UpdateMulti(products, insertIfNoExists));
        }

        /// <summary>
        /// Metodo para borrar un producto
        /// </summary>
        /// <param name="product">Id Producto para borrar</param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DelProduct([FromQuery]string id)
        {
            return Ok(pBS.Delete(id));
        }

        /// <summary>
        /// Metodo para borrar varios productos
        /// </summary>
        /// <param name="products">Listado de id productos</param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DelProductRange([FromBody]IEnumerable<string> products)
        {
            return Ok(pBS.DeleteMulti(products));
        }
        #endregion
    }
}