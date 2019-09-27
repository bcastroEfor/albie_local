using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albie.Models
{
    public class HojaRecuentoGroup
    {
        public string Name { get; set; }
        public IGrouping<string,HojaRecuento> Items { get; set; }
    }
}
