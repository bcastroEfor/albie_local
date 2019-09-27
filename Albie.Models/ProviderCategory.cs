using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class ProviderCategory
    {
        [Key]
        public string VendorNo { get; set; }
        public Provider Provider { get; set; }
        [Key]
        [Column("ItemCategoryCode")]
        public string Code { get; set; }
        //public ICollection<ProductCategory> ProductCategory { get; set; }
    }
}
