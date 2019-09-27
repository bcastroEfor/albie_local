using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class CartList
    {
        [Key]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public DateTimeOffset F_Creacion { get; set; }
        public decimal TotalPrice { get; set; }
        public bool? PedidoHabitual { get; set; }
        public ICollection<ProductList> ProductList { get; set; }
    }
}
