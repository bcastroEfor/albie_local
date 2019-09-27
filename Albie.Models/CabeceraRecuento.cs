using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Albie.Models
{
    public class CabeceraRecuento
    {
        [Key]
        public int IdRecuento { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
