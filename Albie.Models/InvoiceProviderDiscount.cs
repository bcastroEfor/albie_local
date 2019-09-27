using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class InvoiceProviderDiscount
    {
        [Key]
        public string Code { get; set; }
        public Provider Provider { get; set; }
        public decimal? MinimumAccount { get; set; }
        public decimal? Discount { get; set; }
    }
}
