using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class Line
    {
        [Key]
        public int LineNo { get; set; }
        public int DocumentNo { get; set; }
        [NotMapped]
        public Document Order { get; set; }
        public string No { get; set; }
        [NotMapped]
        public Product Product { get; set; }
        public string LocationCode { get; set; }
        [NotMapped]
        public Location Location { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        [NotMapped]
        public UnitMeasureProduct UnitMeasureProduct { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? DirectUnitCost { get; set; }
        public decimal? VAT { get; set; }
        public decimal? LineDiscount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountIncludingVAT { get; set; }
        public DateTimeOffset? ExpectedReceiptDate { get; set; }
        public decimal? OutstandingQuantity { get; set; }
        public decimal? QuantityToReceive { get; set; }
        public decimal? QuantityReceived { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
    }
}
