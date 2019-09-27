using System;
using System.Collections.Generic;
using System.Text;

namespace Albie.Models
{
    public class MatSelect<TValue>
    {
        public TValue Value { get; set; }
        public string ViewValue { get; set; }


        public MatSelect(string label, TValue value)
        {
            Value = value;
            ViewValue = label;
        }
    }
}
