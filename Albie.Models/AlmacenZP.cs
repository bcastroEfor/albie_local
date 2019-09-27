using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class AlmacenZP
    {
        [Key]
        public string LocationCode { get; set; }
        public Location Location { get; set; }
        public string Zona { get; set; }
        public Zona Zonas { get; set; }
        [Key]
        public string ProductNo { get; set; }
        public Product Product { get; set; }
    }
}
