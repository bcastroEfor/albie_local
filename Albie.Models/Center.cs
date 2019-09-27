using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class Center
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string LocationCode { get; set; }
        public int? ConfiguracionRecuentoId { get; set; }
        [NotMapped]
        public Location Location { get; set; }
        public string GlobalDimension1Code { get; set; }
        [NotMapped]
        public Dimension Centro { get; set; }
        public string GlobalDimension2Code { get; set; }
        [NotMapped]
        public Dimension Zona { get; set; }
        public string DefaultCustomerNo { get; set; }
        [NotMapped]
        public Customer DefaultCustomer { get; set; }
        public string PrincipalCustomerNo { get; set; }
        [NotMapped]
        public Customer PrincipalCustomer { get; set; }
        public ICollection<HojaRecuento> HojaRecuentos { get; set; }


        public virtual ICollection<SalesCenter> SaleCenters { get; set; }
        public virtual ICollection<Subcenter> Subcenters { get; set; }
    }
}
