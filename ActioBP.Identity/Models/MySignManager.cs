using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ActioBP.Identity.Models
{
    public class MySignInManager : SignInManager<MyUser>
    {
        public MySignInManager(UserManager<MyUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<MyUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<MyUser>> logger,
            IAuthenticationSchemeProvider schemes) :
            base(userManager,contextAccessor,claimsFactory,optionsAccessor,logger,schemes) { }     
    }
}
