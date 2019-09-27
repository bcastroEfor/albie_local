using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class DiscountLineInvoice
    {
        [Key]
        public string ItemNo { get; set; }
        [NotMapped]
        public Product Product { get; set; }
        public string VendorNo { get; set; }
        [NotMapped]
        public Provider Provider { get; set; }
        public DateTimeOffset? StartingDate { get; set; }
        public decimal? LineDiscount { get; set; }
        public decimal? MinimumQuantity { get; set; }
        public DateTimeOffset? EndingDate { get; set; }
        public string UnitMeasureCode { get; set; }
        [NotMapped]
        public UnitMeasureProduct UnitMeasureProduct { get; set; }
    }
}
