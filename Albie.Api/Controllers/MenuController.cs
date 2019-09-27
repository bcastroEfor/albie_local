using Albie.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Albie.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/menu")]
    public class MenuController : BaseController
    {
        public MenuController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<WebMenu_View> menu = new List<WebMenu_View>();
            List<WebMenu_View> albaran = new List<WebMenu_View>();
            List<WebMenu_View> pedidos = new List<WebMenu_View>();
            List<WebMenu_View> ventas = new List<WebMenu_View>();


            #region Escritorio
            menu.Add(new WebMenu_View()
            {
                Label = "Catálogo",
                IconClass = "fa fa-shopping-cart",
                RouterLink = "/compra",
                RouterLinkActive = ""
            });
            menu.Add(new WebMenu_View()
            {
                Label = "Lista de compra",
                IconClass = "fa fa-file-text-o",
                RouterLink = "/lista",
                RouterLinkActive = ""
            });
            menu.Add(new WebMenu_View()
            {
                Label = "Pedidos",
                IconClass = "fa fa-file-text-o",
                Children = pedidos
            });

            menu.Add(new WebMenu_View()
            {
                Label = "Albaranes",
                IconClass = "fa fa-file-text-o",
                Children = albaran
            });


            menu.Add(new WebMenu_View()
            {
                Label = "Alta de proveedor",
                IconClass = "fa fa-user-plus",
                RouterLink = "/proveedor/detalle",
                RouterLinkActive = ""
            });
            menu.Add(new WebMenu_View()
            {
                Label = "Inventario",
                IconClass = "fa fa-th",
                RouterLink = "/inventario",
                RouterLinkActive = ""
            });
            menu.Add(new WebMenu_View()
            {
                Label = "Iniciar recuento",
                IconClass = "fa fa-th",
                RouterLink = "/recuento/detalle",
                RouterLinkActive = ""
            });

            menu.Add(new WebMenu_View()
            {
                Label = "Ventas",
                IconClass = "fa fa-th",
                Children = ventas
            });
            #endregion

            #region Pedidos
            pedidos.Add(new WebMenu_View()
            {
                Label = "Pedidos habituales",
                IconClass = "fa fa-star",
                RouterLink = "/pedidos-habituales",
                RouterLinkActive = ""
            });
            pedidos.Add(new WebMenu_View()
            {
                Label = "Pedidos",
                IconClass = "fa fa-file-text-o",
                RouterLink = "/order",
                RouterLinkActive = ""
            });
            pedidos.Add(new WebMenu_View()
            {
                Label = "Histórico de pedidos",
                IconClass = "fa fa-history",
                RouterLink = "/order/historico",
                RouterLinkActive = ""
            });
            #endregion

            #region Albaranes
            albaran.Add(new WebMenu_View()
            {
                Label = "Recepción mercancía",
                IconClass = "fa fa-th",
                RouterLink = "/albaran/detalle",
                RouterLinkActive = ""
            });

            albaran.Add(new WebMenu_View()
            {
                Label = "Listado albaranes",
                IconClass = "fa fa-list",
                RouterLink = "/albaran/lista",
                RouterLinkActive = ""
            });
            #endregion

            #region Ventas
            ventas.Add(new WebMenu_View()
            {
                Label = "Ventas centro",
                IconClass = "fa fa-home",
                RouterLink = "/ventas/centro",
                RouterLinkActive = ""
            });

            ventas.Add(new WebMenu_View()
            {
                Label = "Ventas contado",
                IconClass = "fa fa-th",
                RouterLink = "/ventas/contado",
                RouterLinkActive = ""
            });

            ventas.Add(new WebMenu_View()
            {
                Label = "Ventas caja",
                IconClass = "fa fa-th",
                RouterLink = "/ventas/caja",
                RouterLinkActive = ""
            });

            ventas.Add(new WebMenu_View()
            {
                Label = "Resumen cajas",
                IconClass = "fa fa-th",
                RouterLink = "/ventas/resumen",
                RouterLinkActive = ""
            });
            #endregion
            return Ok(menu);
        }
    }
}