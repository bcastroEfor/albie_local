using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class ValueString
    {
        [Key]
        public string Value { get; set; }
    }
}