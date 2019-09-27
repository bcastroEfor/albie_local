using Albie.Models;
using System;

namespace Albie.Api.ViewModels
{
    public class CashMovementCenter_View
    {
        public int EntryNo { get; set; }
        public string CenterCode { get; set; }
        public string Type { get; set; }
        public string PaymentMethodCode { get; set; }
        public DateTimeOffset? PostingDate { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public string PostingStatus { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }

        public CashMovementCenter_View(CashMovementCenter c)
        {
            EntryNo = c.EntryNo;
            CenterCode = c.CenterCode ?? "";
            Type = c.Type ?? "";
            PaymentMethodCode = c.PaymentMethodCode ?? "";
            PostingDate = c.PostingDate ?? DateTimeOffset.MinValue;
            Description = c.Description ?? "";
            Amount = c.Amount ?? 0;
            PostingStatus = c.PostingStatus ?? "";
            ReadingDate = c.ReadingDate ?? DateTimeOffset.MinValue;
        }
    }
}
