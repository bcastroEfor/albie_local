using Albie.Models;
using System;
using System.Collections.Generic;

namespace Albie.Api.ViewModels
{
    public class Document_View
    {
        public int No { get; set; }
        public string BuyFromVendorNo { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public DateTimeOffset? ExpectedReceiptDate { get; set; }
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
        public int Estado { get; set; }
        public string OrigenPedido { get; set; }
        public decimal? AmountIncludingVAT { get; set; }
        public IEnumerable<Line> Lines { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }

        public Document_View(Document p)
        {
            No = p.No;
            OrderDate = p.OrderDate;
            ExpectedReceiptDate = p.ExpectedReceiptDate ?? DateTimeOffset.MinValue;
            Centro = p.Centro;
            Zona = p.Zona;
            BuyFromVendorName = p.BuyFromVendorName ?? "";
            BuyFromVendorName2 = p.BuyFromVendorName2 ?? "";
            BuyFromAddress = p.BuyFromAddress ?? "";
            BuyFromAddress2 = p.BuyFromAddress2 ?? "";
            BuyFromPostCode = p.BuyFromPostCode ?? "";
            BuyFromCounty = p.BuyFromCounty ?? "";
            BuyFromCity = p.BuyFromCity ?? "";
            BuyFromVendorNo = p.BuyFromVendorNo ?? "";
            BuyFromContact = p.BuyFromContact ?? "";
            Amount = p.Amount;
            VendorOrderNo = p.VendorOrderNo ?? "";
            PostingDate = p.PostingDate ?? DateTimeOffset.MinValue;
            VendorShipmentNo = p.VendorShipmentNo ?? "";
            Estado = p.Estado;
            OrigenPedido = p.OrigenPedido ?? "";
            AmountIncludingVAT = p.AmountIncludingVAT ?? 0;
            Lines = p.Lines;
            ReadingDate = p.ReadingDate ?? DateTimeOffset.MinValue;
        }
    }
}
