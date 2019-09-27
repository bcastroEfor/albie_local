using System;
using System.Collections.Generic;
using System.Text;

namespace Albie.Models
{
    public class Recuento
    {
        public string Almacen { get; set; }
        public List<Zonas> Zonas { get; set; }
    }

    public class Zonas
    {
        public string Zona { get; set; }
        public List<AlmacenZP> Productos { get; set; }
    }
}
