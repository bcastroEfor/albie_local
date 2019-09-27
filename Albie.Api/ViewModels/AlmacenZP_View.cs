using Albie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class AlmacenZP_View
    {
        public string LocationCode { get; set; }
        public Location Location { get; set; }
        public string Zona { get; set; }
        public Zona Zonas { get; set; }
        public string ProductNo { get; set; }
        public Product Product { get; set; }

        public AlmacenZP_View(AlmacenZP a)
        {
            LocationCode = a.LocationCode;
            Zona = a.Zona;
            ProductNo = a.ProductNo;
            Location = a.Location;
            Zonas = a.Zonas;
            Product = a.Product;
        }

        public IEnumerable<AlmacenZP> Almacenes { get; set; }
        public string Almacen { get; set; }
        public AlmacenZP_View(IGrouping<string, AlmacenZP> almacenes)
        {
            Almacenes = almacenes.Where(o => o.LocationCode == almacenes.Key);
            Almacen = almacenes.Where(o => o.LocationCode == almacenes.Key).First().Location.Name;
        }
    }
}
