using Albie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class Lines_View
    {
        public int LineNo { get; set; }
        public int DocumentNo { get; set; }
        public string No { get; set; }
        public string LocationCode { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
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

        public Lines_View(Line l)
        {
            LineNo = l.LineNo;
            DocumentNo = l.DocumentNo;
            No = l.No;
            LocationCode = l.LocationCode ?? "";
            Description = l.Description ?? "";
            UnitOfMeasure = l.UnitOfMeasure ?? "";
            Quantity = l.Quantity ?? 0;
            DirectUnitCost = l.DirectUnitCost ?? 0;
            VAT = l.VAT ?? 0;
            LineDiscount = l.LineDiscount ?? 0;
            Amount = l.Amount ?? 0;
            AmountIncludingVAT = l.AmountIncludingVAT ?? 0;
            ExpectedReceiptDate = l.ExpectedReceiptDate ?? DateTimeOffset.MinValue;
            OutstandingQuantity = l.OutstandingQuantity ?? 0;
            QuantityReceived = l.QuantityReceived ?? 0;
            QuantityToReceive = l.QuantityToReceive ?? 0;
        }
    }
}
