using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albie.Models
{
    public class VentasCentro<T>
    {
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<LabelAndValue<DateTime>> Dates { get; set; }
    }
}
