using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Albie.Models
{
    public class CashMovementCenter
    {
        [Key]
        public int EntryNo { get; set; }
        public string CenterCode { get; set; }
        public Center Center { get; set; }
        public string SubcenterType { get; set; }
        public string SubcenterCode { get; set; }
        public string Type { get; set; }
        public string PaymentMethodCode { get; set; }
        public DateTimeOffset? PostingDate { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public string PostingStatus { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
    }
}
