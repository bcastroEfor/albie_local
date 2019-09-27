using Albie.Models;
using System.Linq;

namespace Albie.Api.ViewModels
{
    public class VentasContado_View : Ventas_View<Center_View>
    {
        public VentasContado_View(VentasCentro<Center> v)
        {
            Items = v.Items.Select(o => new Center_View(o));
            Dates = v.Dates;
        }
    }
}
