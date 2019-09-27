using Albie.Models;
using System.Collections.Generic;

namespace Albie.Api.ViewModels
{
    public class Center_View
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string LocationCode { get; set; }
        public int? ConfiguracionRecuentoId { get; set; }
        public Location Location { get; set; }
        public string GlobalDimension1Code { get; set; }
        public Dimension Centro { get; set; }
        public string GlobalDimension2Code { get; set; }
        public Dimension Zona { get; set; }
        public string DefaultCustomerNo { get; set; }
        public Customer DefaultCustomer { get; set; }
        public string PrincipalCustomerNo { get; set; }
        public Customer PrincipalCustomer { get; set; }
        public ICollection<HojaRecuento> HojaRecuentos { get; set; }


        public virtual ICollection<SalesCenter> SaleCenters { get; set; }
        public virtual ICollection<Subcenter> Subcenters { get; set; }

        public Center_View(Center o)
        {
            Code = o.Code;
            Description = o.Description;
            Name = o.Name;
            Address = o.Address;
            Address2 = o.Address2;
            LocationCode = o.LocationCode;
            ConfiguracionRecuentoId = o.ConfiguracionRecuentoId;
            Location = o.Location;
            GlobalDimension1Code = o.GlobalDimension1Code;
            GlobalDimension2Code = o.GlobalDimension2Code;
            Centro = o.Centro;
            Zona = o.Zona;
            DefaultCustomer = o.DefaultCustomer;
            DefaultCustomerNo = o.DefaultCustomerNo;
            PrincipalCustomer = o.PrincipalCustomer;
            PrincipalCustomerNo = o.PrincipalCustomerNo;
            HojaRecuentos = o.HojaRecuentos;
            Subcenters = o.Subcenters;
        }
    }
}