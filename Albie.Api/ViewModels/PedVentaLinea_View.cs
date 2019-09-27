using Albie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class PedVentaLinea_View
    {
        public string DocumentNo { get; set; }
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

        public PedVentaLinea_View(PedVentaLinea p)
        {
            DocumentNo = p.DocumentNo;
            LineNo = p.LineNo;
            No = p.No ?? "";
            Description = p.Description ?? "";
            UnitOfMeasure = p.UnitOfMeasure ?? "";
            Quantity = p.Quantity ?? 0;
            UnitPrice = p.UnitPrice ?? 0;
            VAT = p.VAT ?? 0;
            LineDiscount = p.LineDiscount ?? 0;
            Amount = p.Amount ?? 0;
            AmountIncludingVAT = p.AmountIncludingVAT ?? 0;
            ReadingDate = p.ReadingDate ?? DateTimeOffset.MinValue;
        }
    }
}
