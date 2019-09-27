using Albie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class Ventas_View<T>
    {
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<LabelAndValue<DateTime>> Dates { get; set; }
    }
}
