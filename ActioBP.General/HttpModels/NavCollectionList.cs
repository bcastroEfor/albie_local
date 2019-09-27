using System.Collections.Generic;
using System.Linq;

namespace ActioBP.General.HttpModels
{
    public class NavCollectionList<T>
    {
        private int? _Total;

        public int Total
        {
            get { return _Total.HasValue ? _Total.Value : Items.Count(); }
            set { _Total = value; }
        }

        public IEnumerable<T> Items { get; set; }
        public string PageMarkPrevious { get; set; }
        public string PageMarkNext { get; set; }

        public NavCollectionList()
        {
            Items = new List<T>();
        }
        public NavCollectionList(IEnumerable<T> itemsCollection, int? totalItems = null)
        {
            Items = itemsCollection;
            _Total = totalItems;
        }
    }
}
