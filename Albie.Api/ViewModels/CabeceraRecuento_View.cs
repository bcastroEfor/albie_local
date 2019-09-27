using Albie.Models;

namespace Albie.Api.ViewModels
{
    public class CabeceraRecuento_View
    {
        public int IdRecuento { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public CabeceraRecuento_View(CabeceraRecuento c)
        {
            IdRecuento = c.IdRecuento;
            Name = "HR" + c.IdRecuento;
            Status = Status = c.Status == 0 ? "Pendiente" : "Cerrado";
        }
    }
}
