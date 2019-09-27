using ActioBP.Linq.FilterLinq;
using System;
using System.Collections.Generic;

namespace ActioBP.Linq.Nav.Datatable
{

    public class  NavFilter<TField> where TField : struct, IConvertible
    {
        public TField Field { get; set; }
        public string Criteria { get; set; }
        public NavFilter()
        {
        }

    }
}
