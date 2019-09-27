using System;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class PedVentaCab
    {
        [Key]
        public string No { get; set; }
        public string BillToCustomerNo { get; set; }
        public string BillToName { get; set; }
        public string BillToName2 { get; set; }
        public string BillToAddress { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToCity { get; set; }
        public string BillToContact { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public string VATRegistrationNo { get; set; }
        public string BillToPostCode { get; set; }
        public string BillToCounty { get; set; }
        public string BillToCountry { get; set; }
        public DateTimeOffset? DocumentDate { get; set; }
        public string Comment { get; set; }
        public string BillToContactTelefono { get; set; }
        public DateTimeOffset? ReadingDate { get; set; }
    }
}
