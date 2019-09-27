using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class Location
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<Line> Lines { get; set; }
        public ICollection<AlmacenZP> AlmacenZP { get; set; }
        public ICollection<HojaRecuento> HojaRecuentos { get; set; }
    }
}
