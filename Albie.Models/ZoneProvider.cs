using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class ZoneProvider
    {
        [Key]
        public string VendorNo { get; set; }
        public Provider Provider { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public Center Centro { get; set; }
    }
}
