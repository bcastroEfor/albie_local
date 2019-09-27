using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class Dimension
    {
        [Key]
        public string Code { get; set; }
        public string DimensionCode { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string CustomerNo { get; set; }
        [NotMapped]
        public string ProviderVendorNo { get; set; }
        [NotMapped]
        public string SubcenterCode { get; set; }
        [NotMapped]
        public string DocumentNo { get; set; }
        [NotMapped]
        public ICollection<Customer> CenterCustomers { get; set; }
        [NotMapped]
        public ICollection<Customer> ZoneCustomers { get; set; }
        [NotMapped]
        public ICollection<Provider> CenterProvider { get; set; }
        [NotMapped]
        public ICollection<Provider> ZoneProvider { get; set; }
        [NotMapped]
        public ICollection<Center> Centers { get; set; }
        [NotMapped]
        public ICollection<Center> ZoneCenter { get; set; }
        [NotMapped]
        public ICollection<Subcenter> SubCenterCenter { get; set; }
        [NotMapped]
        public ICollection<Subcenter> SubCenterZone { get; set; }
        [NotMapped]
        public ICollection<Document> DocumentCenter { get; set; }
        [NotMapped]
        public ICollection<Document> DocumentZone { get; set; }
    }
}
