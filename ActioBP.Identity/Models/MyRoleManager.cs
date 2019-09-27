using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ActioBP.Identity.Models
{
    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class MyRoleManager : RoleManager<MyRole>
    {
        public MyRoleManager(IRoleStore<MyRole> roleStore,
            IEnumerable<IRoleValidator<MyRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<MyRole>> logger
            )
            : base(roleStore, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
