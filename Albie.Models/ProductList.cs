using System;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class ProductList
    {
        [Key]
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public string ProductId { get; set; }
        public Guid ListId { get; set; }
        public CartList List { get; set; }
        public decimal Quantity { get; set; }
        public Guid ProviderRateId { get; set; }
        public ProviderRate ProviderRate { get; set; }
        public string ProviderId { get; set; }
        public Provider Provider { get; set; }
        public decimal? QuantityReceived { get; set; }
    }
}
