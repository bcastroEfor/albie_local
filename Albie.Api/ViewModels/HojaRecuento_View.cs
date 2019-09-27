using Albie.Models;
using System;

namespace Albie.Api.ViewModels
{
    public class HojaRecuento_View
    {
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

        public HojaRecuento_View(HojaRecuento h)
        {
            EntryNo = h.EntryNo;
            Codigo = h.Codigo;
            Date = h.Date;
            CenterCode = h.CenterCode;
            LocationCode = h.LocationCode;
            Location = h.Location;
            Zone = h.Zone;
            Zona = h.Zona;
            ProductNo = h.ProductNo;
            Product = h.Product;
            Quantity = h.Quantity;            
        }
    }
}
