using System;
using System.ComponentModel.DataAnnotations;

namespace ActioBP.Identity.Models
{
    public class AppRefreshToken
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(100)]
        public string AppClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        [Required]
        public string ProtectedTicket { get; set; }
    }
}