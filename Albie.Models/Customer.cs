using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class Customer
    {
        [Key]
        public string No { get; set; }
        public string Name { get; set; }
        public string Nam2 { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
        public string PhoneNo { get; set; }
        public string GlobalDimension1Code { get; set; }
        public Dimension Centro { get; set; }
        public string GlobalDimension2Code { get; set; }
        public Dimension Zona { get; set; }
        public string Country { get; set; }
        public string Blocked { get; set; }
        public string VATRegistrationNo { get; set; }
        public string PostCode { get; set; }
        public string County { get; set; }
        public string Email { get; set; }
        public string HomePage { get; set; }
        public ICollection<CustomerCenter> CustomerCenters { get; set; }
        public ICollection<CustomerRate> CustomerRates { get; set; }
        public virtual ICollection<SalesCenter> SalesCenters { get; set; }
    }
}
