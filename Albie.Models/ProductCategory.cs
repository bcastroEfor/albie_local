using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class ProductCategory
    {
        [Key]
        public string Code { get; set; }
        public string ParentCategory { get; set; }
        public string Description { get; set; }
        public int? Identation { get; set; }
        public ICollection<Product> Product { get; set; }
        public ICollection<FamilyProvider> FamilyProviders { get; set; }
        //public ProductCategory CategoryProduct { get; set; }
        //public ProviderCategory ProviderCategory { get; set; }
    }

}
