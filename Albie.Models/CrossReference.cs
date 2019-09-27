using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class CrossReference
    {
        [Key]
        public string ItemNo { get; set; }
        public string UnitOfMeasure { get; set; }
        public string CrossReferenceTypeNo { get; set; }
        public string CrossReferenceNo { get; set; }
        public string Description { get; set; }
    }
}
