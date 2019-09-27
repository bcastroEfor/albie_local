using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ActioBP.Identity.Models
{

    public class MyDbContext : IdentityDbContext<MyUser,MyRole,Guid>
    {
        #region constructors and destructors
        public MyDbContext(DbContextOptions options) : base(options) { }        
        public MyDbContext():base(){ }
        #endregion

        public DbSet<AppClient> AppClient { get; set; }
        public DbSet<AppRefreshToken> AppRefreshToken { get; set; }
    }
}