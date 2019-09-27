using Albie.Models;
using System.Linq;

namespace Albie.Api.ViewModels
{
    public class VentasCentro_View : Ventas_View<CustomerRate_View>
    {
        public VentasCentro_View(VentasCentro<CustomerRate> v)
        {
            Items = v.Items.Select(o => new CustomerRate_View(o));
            Dates = v.Dates;
        }
    }
}
