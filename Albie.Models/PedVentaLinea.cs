using System;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class PedVentaLinea
    {
        [Key]
        public string DocumentNo { get; set; }
        [Key]
        public int LineNo { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? VAT { get; set; }
        public decimal? LineDiscount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountIncludingVAT { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
    }
}
