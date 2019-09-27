using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActioBP.General.HttpModels
{
    public class CollectionList<T>
    {
        private int? _Total;

        public int Total
        {
            get { return _Total.HasValue ? _Total.Value : Items.Count(); }
            set { _Total = value; }
        }

        public IEnumerable<T> Items { get; set; }

        public CollectionList() {
            Items = new List<T>();
        }
        public CollectionList(IEnumerable<T> itemsCollection, int? totalItems=null)
        {
            Items = itemsCollection;
            _Total = totalItems;
        }
    }
}
