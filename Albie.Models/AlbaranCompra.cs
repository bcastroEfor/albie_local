using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class AlbaranCompra
    {
        [Key]
        public string No { get; set; }
        public string BuyFromVendorNo { get; set; }
        public string VendorShipmentNo { get; set; }
        public DateTimeOffset? PostingDate { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public string BuyFromVendorName { get; set; }
        public string BuyFromVendorName2 { get; set; }
        public string BuyFromAddress { get; set; }
        public string BuyFromAddress2 { get; set; }
        public string BuyFromCity { get; set; }
        public string BuyFromContact { get; set; }
        public string BuyFromPostCode { get; set; }
        public string BuyFromCounty { get; set; }
        public decimal? Amount { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
        public string OrderNo { get; set; }
        public bool? NonConform { get; set; }
        public bool? Anulado { get; set; }
        public ICollection<AlbaranLinea> AlbaranLineas { get; set; }
    }
}
