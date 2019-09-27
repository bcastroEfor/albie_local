using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Albie.Models
{
    public class Provider 
    {
        [Key]
        public string VendorNo { get; set; }
        [NotMapped]
        public ProviderCategory ProviderCategory { get; set; }
        public string Name { get; set; }
        public string SearchName { get; set; }
        public string Name2 { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
        public string PhoneNo { get; set; }
        public string OurAccountNo { get; set; }
        public string GlobalDimension1Code { get; set; }
        [NotMapped]
        public Dimension Centro { get; set; }
        public string GlobalDimension2Code { get; set; }
        [NotMapped]
        public Dimension Zona { get; set; }
        public string ShippingAgentCode { get; set; }
        public string CountryRegionCode { get; set; }
        public bool? Blocked { get; set; }
        public string PaymentMethodCode { get; set; }
        public string VATRegistrationNo { get; set; }
        public string PostCode { get; set; }
        public string County { get; set; }
        public string Email { get; set; }
        public string HomePage { get; set; }
        [NotMapped]
        public ProviderRate ProviderRate { get; set; }
        [NotMapped]
        public ICollection<ZoneProvider> ZoneProviders { get; set; }
        [NotMapped]
        public ICollection<FamilyProvider> FamilyProviders { get; set; }
        [NotMapped]
        public InvoiceProviderDiscount InvoiceProviderDiscount { get; set; }
        [NotMapped]
        public ICollection<Document> HeaderOrder { get; set; }
        [NotMapped]
        public ICollection<DiscountLineInvoice> DiscountLineInvoice { get; set; }
    }
}
