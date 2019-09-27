using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class HojaRecuento
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryNo { get; set; }
        public string Codigo { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string CenterCode { get; set; }
        public Center Center { get; set; }
        public string LocationCode { get; set; }
        public Location Location { get; set; }
        public string Zone { get; set; }
        public Zona Zona { get; set; }
        public string ProductNo { get; set; }
        public Product Product { get; set; }
        public decimal? Quantity { get; set; }
    }
}
