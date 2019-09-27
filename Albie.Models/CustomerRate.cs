using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class CustomerRate
    {
        [Key]
        public string ItemNo { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
        public DateTimeOffset? StartingDate { get; set; }
        public DateTimeOffset? EndingDate { get; set; }
        public decimal? UnitPrice { get; set; }
        public string SalesType { get; set; }
        public decimal? MinimumQuantity { get; set; }
        public string UnitMeasureCode { get; set; }
        public UnitMeasureProduct UnitMeasureProduct { get; set; }


        public string SalesCode { get; set; }
        public ICollection<SalesCenter> SalesCenters { get; set; }
    }
}
