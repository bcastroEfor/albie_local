using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class UnitMeasureProduct
    {
        [Key]
        [Column("ItemNo")]
        public string ProductNo { get; set; }
        public string Code { get; set; }
        public decimal? Quantity { get; set; }
        public Product Product { get; set; }
        public ProviderRate ProviderRate { get; set; }
        public ICollection<CustomerRate> CustomerRates { get; set; }
        public ICollection<Line> Lines { get; set; }
    }
}
