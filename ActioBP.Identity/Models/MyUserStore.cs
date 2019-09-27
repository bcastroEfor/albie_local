using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace ActioBP.Identity.Models
{
    public class MyUserStore<TUser> : UserStore<TUser, MyRole, MyDbContext, Guid>
        where TUser : IdentityUser<Guid>
    {
        public MyUserStore(MyDbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }
}
