using Albie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class PedVentaCab_View
    {
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

        public PedVentaCab_View(PedVentaCab p)
        {
            No = p.No;
            BillToCustomerNo = p.BillToCustomerNo;
            BillToName = p.BillToName ?? "";
            BillToName2 = p.BillToName2 ?? "";
            BillToAddress = p.BillToAddress ?? "";
            BillToAddress2 = p.BillToAddress2 ?? "";
            BillToCity = p.BillToCity ?? "";
            BillToContact = p.BillToContact ?? "";
            OrderDate = p.OrderDate ?? DateTimeOffset.MinValue;
            ShortcutDimension1Code = p.ShortcutDimension1Code ?? "";
            ShortcutDimension2Code = p.ShortcutDimension2Code ?? "";
            VATRegistrationNo = p.VATRegistrationNo ?? "";
            BillToPostCode = p.BillToPostCode ?? "";
            BillToCounty = p.BillToCounty ?? "";
            BillToCountry = p.BillToCountry ?? "";
            DocumentDate = p.DocumentDate ?? DateTimeOffset.MinValue;
            Comment = p.Comment ?? "";
            BillToContactTelefono = p.BillToContactTelefono ?? "";
            ReadingDate = p.ReadingDate ?? DateTimeOffset.MinValue;
        }
    }
}
