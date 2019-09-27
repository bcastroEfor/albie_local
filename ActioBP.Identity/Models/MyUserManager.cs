using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;

namespace ActioBP.Identity.Models
{
    public class MyUserManager : UserManager<MyUser>
    {
        public MyUserManager(IUserStore<MyUser> store,
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<MyUser> passwordHasher, 
            IEnumerable<IUserValidator<MyUser>> userValidators, 
            IEnumerable<IPasswordValidator<MyUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<UserManager<MyUser>> logger)
            : base(store,optionsAccessor,passwordHasher,userValidators,passwordValidators, keyNormalizer, errors,services, logger)
        {
            this.PasswordHasher = new MyPasswordHasher();            
        }        
    }
}