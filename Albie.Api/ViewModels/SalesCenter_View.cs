using Albie.Models;
using System;
using System.Collections.Generic;

namespace Albie.Api.ViewModels
{
    public class SalesCenter_View
    {
        public int EntryNo { get; set; }
        public string CenterCode { get; set; }
        public string CustomerNo { get; set; }
        public string ItemNo { get; set; }
        public string PostingDate { get; set; }
        public decimal? Quantity { get; set; }
        public string PostingStatus { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
        public decimal? Amount { get; set; }

        public SalesCenter_View(SalesCenter s)
        {
            EntryNo = s.EntryNo;
            CenterCode = s.CenterCode ?? "";
            CustomerNo = s.CustomerNo ?? "";
            ItemNo = s.ItemNo ?? "";
            PostingDate = s.PostingDate.Value.ToString("dd MMMM dddd") ?? DateTimeOffset.MinValue.ToString();
            Quantity = s.Quantity ?? 0;
            PostingStatus = s.PostingStatus ?? "";
            ReadingDate = s.ReadingDate ?? DateTimeOffset.MinValue;
            Amount = s.Amount ?? 0;
        }
    }
}
