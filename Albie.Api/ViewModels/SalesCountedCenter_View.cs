using Albie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class SalesCountedCenter_View
    {
        public int EntryNo { get; set; }
        public string CenterCode { get; set; }
        public string SubcenterType { get; set; }
        public string SubcenterCode { get; set; }
        public DateTimeOffset? PostingDate { get; set; }
        public string Schedule { get; set; }
        public decimal? Amount { get; set; }
        public string InitialTicket { get; set; }
        public string FinalTicket { get; set; }
        public string PostingStatus { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }

        public SalesCountedCenter_View(SalesCountedCenter sc)
        {
            EntryNo = sc.EntryNo;
            CenterCode = sc.CenterCode ?? "";
            SubcenterType = sc.SubcenterType ?? "";
            SubcenterCode = sc.SubcenterCode ?? "";
            PostingDate = sc.PostingDate ?? DateTimeOffset.MinValue;
            Schedule = sc.Schedule ?? "";
            Amount = sc.Amount ?? 0;
            InitialTicket = sc.InitialTicket ?? "";
            FinalTicket = sc.FinalTicket ?? "";
            PostingStatus = sc.PostingStatus ?? "";
            ReadingDate = sc.ReadingDate ?? DateTimeOffset.MinValue;
        }
    }
}
