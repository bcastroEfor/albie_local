using ActioBP.Linq.FilterLinq;
using Albie.BS.Interfaces;
using Albie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albie.Api.Controllers
{
    [Route("api/providerrate/[action]")]
    [ApiController]
    public class ProviderRateController : BaseController
    {
        private IConfiguration _conf;
        private IListBS lBS;
        private IProviderBS pBS;
        private IProviderRateBS prBS;
        public ProviderRateController(IConfiguration conf, IProviderBS provider, IListBS lista, IProviderRateBS providerRate) : base(conf)
        {
            pBS = provider;
            lBS = lista;
            prBS = providerRate;
        }

        #region GET
        /// <summary>
        /// Obtiene un CollectionList de las tarifas de los proveedores
        /// </summary>
        /// <param name="productId">Id del producto para obtener sus tarifas</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCollectionListProvidersRates([FromQuery]string productId, [FromBody]List<FilterCriteria> filter, [FromQuery(Name = "pi")]int pageIndex, [FromQuery(Name = "ps")]int pageSize, [FromQuery(Name = "sn")]string sortName, [FromQuery(Name = "sd")]bool sortDescending)
        {
            return Ok(prBS.GetProviderRatesCollectionList(productId: productId, hasProductIncluded: true, hasProviderIncluded: true, hasUnitMeasureProductIncluded: true, filterArr: filter, pageIndex: pageIndex, pagesize: pageSize, sortName: sortName, sortDescending: sortDescending));
        }

        /// <summary>
        /// Obtiene un listado de los proveedores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProviderSelect()
        {
            IEnumerable<Provider> lista = pBS.GetProviderList();
            return Ok(lista.Select(e => new LabelAndValue<string>(e.Name, e.ProviderCategory.Code, e)).OrderBy(o => o.Label));
        }
        #endregion

        #region POST
        /// <summary>
        /// Metodo para cambiar la tarifa de un producto en una lista
        /// </summary>
        /// <param name="rate">objeto de tarifa</param>
        /// <param name="productId">id del producto</param>
        /// <returns></returns>
        [HttpPost("{productId}")]
        public IActionResult ChangeProductRate([FromBody]ProviderRate rate, Guid productId)
        {
            return Ok(lBS.ChangeProductRate(rate, productId));
        }

        [HttpPost]
        public IActionResult UpdProviderRate([FromBody]ProviderRate providerRate, bool insertIfNoExists = false)
        {
            return Ok(prBS.Update(providerRate, insertIfNoExists));
        }

        [HttpPost]
        public IActionResult UpdProviderRateMulti([FromBody]IEnumerable<ProviderRate> providerRate, bool insertIfNoExists = false)
        {
            return Ok(prBS.UpdateMulti(providerRate, insertIfNoExists));
        }

        [HttpDelete]
        public IActionResult DelProviderRate([FromQuery]string productId, [FromQuery]string providerId)
        {
            return Ok(prBS.Delete(productId, providerId));
        }

        [HttpDelete]
        public IActionResult DelProviderRateMulti([FromBody]IEnumerable<string> providers)
        {
            return Ok(prBS.DeleteMulti(providers));
        }
        #endregion
    }
}