using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class Subcenter
    {
        [Key]
        public string Code { get; set; }
        public string CenterCode { get; set; }
        [NotMapped]
        public Center Center { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string GlobalDimension1Code { get; set; }
        [NotMapped]
        public Dimension Centro { get; set; }
        public string GlobalDimension2Code { get; set; }
        [NotMapped]
        public Dimension Zona { get; set; }
        public string Location { get; set; }
        public virtual ICollection<SalesCountedCenter> SalesCountedCenters { get; set; }
    }
}
