using Albie.Models;
using System;

namespace Albie.Api.ViewModels
{
    public class AlbaranLinea_View
    {
        public string AlbaranCompraNo { get; set; }
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

        public AlbaranLinea_View(AlbaranLinea a)
        {
            AlbaranCompraNo = a.AlbaranCompraNo;
            No = a.No;
            ProductNo = a.ProductNo ?? "";
            Description = a.Description ?? "";
            UnitOfMeasure = a.UnitOfMeasure ?? "";
            Quantity = a.Quantity ?? 0;
            DirectUnitCost = a.DirectUnitCost ?? 0;
            VAT = a.VAT ?? 0;
            LineDiscount = a.LineDiscount ?? 0;
            Amount = a.Amount ?? 0;
            AmountIncludingVAT = a.AmountIncludingVAT ?? 0;
            ExpectedReceiptdate = a.ExpectedReceiptdate ?? DateTimeOffset.MinValue;
            QuantityReceived = a.QuantityReceived ?? 0;
            ReadingDate = a.ReadingDate ?? DateTimeOffset.MinValue;
            OrderNo = a.OrderNo ?? "";
            OrderLineNo = a.OrderLineNo ?? "";
            ExcessReception = a.ExcessReception ?? false;
        }
    }
}
