using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class SalesCenter
    {
        [Key]
        public int EntryNo { get; set; }
        public string ItemNo { get; set; }
        //public Product Product { get; set; }
        public DateTimeOffset? PostingDate { get; set; }
        public decimal? Quantity { get; set; }
        public string PostingStatus { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
        public decimal? Amount { get; set; }


        public string CenterCode { get; set; }
        public virtual Center Center { get; set; }

        public string CustomerNo { get; set; }
        public virtual Customer Customer { get; set; }


        public virtual CustomerRate CustomerRate { get; set; }
    }
}
