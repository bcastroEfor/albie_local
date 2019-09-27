using Albie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class HistoricoPedido_View
    {
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
        public DateTimeOffset? ReadingDate { get; set; }
        public DateTimeOffset? ExpectedReceiptDate { get; set; }

        public HistoricoPedido_View(HistoricoPedido d)
        {
            No = d.No;
            OrderDate = d.OrderDate;
            ExpectedReceiptDate = d.ExpectedReceiptDate ?? DateTimeOffset.MinValue;
            Centro = d.Centro;
            Zona = d.Zona;
            BuyFromVendorName = d.BuyFromVendorName ?? "";
            BuyFromVendorName2 = d.BuyFromVendorName2 ?? "";
            BuyFromAddress = d.BuyFromAddress ?? "";
            BuyFromAddress2 = d.BuyFromAddress2 ?? "";
            BuyFromPostCode = d.BuyFromPostCode ?? "";
            BuyFromCounty = d.BuyFromCounty ?? "";
            BuyFromCity = d.BuyFromCity ?? "";
            BuyFromVendorNo = d.BuyFromVendorNo ?? "";
            BuyFromContact = d.BuyFromContact ?? "";
            Amount = d.Amount;
            VendorOrderNo = d.VendorOrderNo ?? "";
            PostingDate = d.PostingDate ?? DateTimeOffset.MinValue;
            VendorShipmentNo = d.VendorShipmentNo ?? "";
            Estado = d.Estado;
            OrigenPedido = d.OrigenPedido ?? "";
            AmountIncludingVAT = d.AmountIncludingVAT ?? 0;
            ReadingDate = d.ReadingDate ?? DateTimeOffset.MinValue;
        }
    }
}
