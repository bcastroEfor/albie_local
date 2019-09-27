using System;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class HistoricoPedido
    {
        [Key]
        public int No { get; set; }
        public string BuyFromVendorNo { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public string Centro { get; set; }
        public string Zona { get; set; }
        public string BuyFromVendorName { get; set; }
        public string BuyFromVendorName2 { get; set; }
        public string BuyFromAddress { get; set; }
        public string BuyFromAddress2 { get; set; }
        public string BuyFromCity { get; set; }
        public string BuyFromContact { get; set; }
        public string BuyFromPostCode { get; set; }
        public string BuyFromCounty { get; set; }
        public decimal? Amount { get; set; }
        public string VendorOrderNo { get; set; }
        public DateTimeOffset? PostingDate { get; set; }
        public string VendorShipmentNo { get; set; }
        public int? Estado { get; set; }
        public string OrigenPedido { get; set; }
        public decimal? AmountIncludingVAT { get; set; }
        public DateTimeOffset? ExpectedReceiptDate { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
    }
}
