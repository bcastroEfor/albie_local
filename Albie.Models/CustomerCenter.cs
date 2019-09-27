using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class CustomerCenter
    {
        [Key]
        public string CustomerNo { get; set; }
        [NotMapped]
        public Customer Customer { get; set; }
        public string CenterCode { get; set; }
        [NotMapped]
        public Center Center { get; set; }
        public bool PrincipalCustomer { get; set; }
        public string CustomerPriceGroup { get; set; }
    }
}
