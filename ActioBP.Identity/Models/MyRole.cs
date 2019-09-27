namespace ActioBP.Identity.Models
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class MyRole : Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>
    {
        public string Description { get; set; }
        public MyRole() : base() { }
        public MyRole(string name) : base() {
            this.Name = name;
        }        
    }
}
