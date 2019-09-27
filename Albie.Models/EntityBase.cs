using System;
using System.ComponentModel.DataAnnotations;

namespace Albie.Models
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime F_Creacion { get; set; }
        public Guid UsuarioCreacion { get; set; }
        public DateTimeOffset? F_Modificacion { get; set; }
        public Guid? UsuarioModificacion { get; set; }
    }
}
