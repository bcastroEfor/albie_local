using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class FamilyProvider
    {
        [Key]
        public string VendorNo { get; set; }
        public Provider Provider { get; set; }
        public string ItemCategoryCode { get; set; }
        public ProductCategory ProviderCategory { get; set; }
    }
}
