using System;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class SalesCountedCenter
    {
        [Key]
        public int EntryNo { get; set; }
        public string CenterCode { get; set; }
        public Center Center { get; set; }
        public string SubcenterType { get; set; }
        public string SubcenterCode { get; set; }
        public Subcenter Subcenter { get; set; }
        public DateTimeOffset? PostingDate { get; set; }
        public string Schedule { get; set; }
        public decimal? Amount { get; set; }
        public string InitialTicket { get; set; }
        public string FinalTicket { get; set; }
        public string PostingStatus { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
    }
}
