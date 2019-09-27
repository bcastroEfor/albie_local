using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class ProviderRate
    {
        [Key]
        //[Column("ItemNo")]
        public string ProductNo { get; set; }
        public Product Product { get; set; }
        [Key]
        public string VendorNo { get; set; }
        public Provider Provider { get; set; }
        public DateTimeOffset? StartingDate { get; set; }
        public decimal? DirectUnitCost { get; set; }
        public decimal? MinimunQuantity { get; set; }
        public DateTimeOffset? EndingDate { get; set; }
        public string UnitMeasureCode { get; set; }
        public UnitMeasureProduct UnitMeasureProduct { get; set; }
        public string VariantCode { get; set; }
        [NotMapped]
        public Guid Id { get; set; }
        [NotMapped]
        public ICollection<ProductList> ProductLists { get; set; }
    }
}
