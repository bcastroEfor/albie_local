using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class Zona
    {
        public string Name { get; set; }
        [Key]
        public string Code { get; set; }
        public ICollection<AlmacenZP> AlmacenZP { get; set; }
        public string LocationCode { get; set; }
        public Location Almacen { get; set; }
        public ICollection<HojaRecuento> HojaRecuentos { get; set; }
    }
}
