using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Albie.Models
{
    public class AlbaranLinea
    {
        [Key]
        public string AlbaranCompraNo { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }
        public string ProductNo { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? DirectUnitCost { get; set; }
        public decimal? VAT { get; set; }
        public decimal? LineDiscount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountIncludingVAT { get; set; }
        public DateTimeOffset? ExpectedReceiptdate { get; set; }
        public decimal? QuantityReceived { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
        public string OrderNo { get; set; }
        public string OrderLineNo { get; set; }
        public bool? ExcessReception { get; set; }
    }
}
