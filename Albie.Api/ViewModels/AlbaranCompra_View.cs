using Albie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class AlbaranCompra_View
    {
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

        public AlbaranCompra_View(AlbaranCompra a)
        {
            No = a.No;
            BuyFromVendorNo = a.BuyFromVendorNo ?? "";
            VendorShipmentNo = a.VendorShipmentNo ?? "";
            PostingDate = a.PostingDate ?? DateTimeOffset.MinValue;
            OrderDate = a.OrderDate ?? DateTimeOffset.MinValue;
            ShortcutDimension1Code = a.ShortcutDimension1Code ?? "";
            ShortcutDimension2Code = a.ShortcutDimension2Code ?? "";
            BuyFromVendorName = a.BuyFromVendorName ?? "";
            BuyFromVendorName2 = a.BuyFromVendorName2 ?? "";
            BuyFromAddress = a.BuyFromAddress ?? "";
            BuyFromAddress2 = a.BuyFromAddress2 ?? "";
            BuyFromCity = a.BuyFromCity ?? "";
            BuyFromContact = a.BuyFromContact ?? "";
            BuyFromPostCode = a.BuyFromPostCode ?? "";
            BuyFromCounty = a.BuyFromCounty ?? "";
            Amount = a.Amount ?? 0;
            ReadingDate = a.ReadingDate ?? DateTimeOffset.MinValue;
            NonConform = a.NonConform ?? false;
            Anulado = a.Anulado ?? false;
            OrderNo = a.OrderNo ?? "";
            AlbaranLineas = a.AlbaranLineas;
        }
    }
}
