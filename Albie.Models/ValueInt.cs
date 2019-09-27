using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Albie.Models
{
    public class ValueInt
    {
        [Key]
        public int Value { get; set; }
    }
}
