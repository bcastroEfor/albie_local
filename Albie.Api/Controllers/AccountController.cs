using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ActioBP.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Albie.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IConfiguration Configuration;
        private readonly MySignInManager SignInManager;
        private readonly MyUserManager UserManager;
        private readonly IUserStore<MyUser> UserStore;
        private readonly IUserEmailStore<MyUser> EmailStore;

        public AccountController(IConfiguration configuration, MySignInManager signInManager, MyUserManager userManager,
            IUserStore<MyUser> userStore)
            : base(configuration)
        {
            Configuration = configuration;
            SignInManager = signInManager;
            UserManager = userManager;
            UserStore = userStore;

            if (UserManager.SupportsUserEmail)
            {
                EmailStore = (IUserEmailStore<MyUser>)UserStore;
            }
            else
            {
                throw new Exception("Soporte de Emails sin habilitar");
            }

        }

        [HttpPost("login"), AllowAnonymous]
        public IActionResult LogIn([FromBody]object aaa)
        {
            return Ok(aaa);
        }

        //[HttpGet, AllowAnonymous]
        //public IActionResult Get()
        //{
        //    if (!User.Identity.IsAuthenticated) return Unauthorized();
        //    // Puedes devolver aquí un User_View si lo necesitas
        //    return Ok();
        //}
        [HttpPost("oauth"), AllowAnonymous]
        public IActionResult SignIn()
        {
            var properties = SignInManager.ConfigureExternalAuthenticationProperties("Microsoft", "/");
            properties.RedirectUri = "/api/account/oauth/test";
            return Challenge(properties, "Microsoft");
        }
        [HttpGet("oauth/test"), AllowAnonymous]
        public async Task<IActionResult> SignInCompleteAsync()
        {
            string returnUrl = "/";
            //returnUrl = returnUrl ?? "/";
            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // El login no es válido o no se ha completado
                return BadRequest();
            }

            //var userid = info.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            //var name = info.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            //var identityAtDb = await UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            //if (identityAtDb == null)
            //{
            //    identityAtDb = await UserManager.FindByEmailAsync(email);
            //    if(identityAtDb == null)
            //    {
            //        identityAtDb = new MyUser();
            //        identityAtDb.Email = email;
            //    }
            //}
            //var userAtIdentity = await UserManager.FindByEmailAsync


            var result = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return Ok();
            }
            if (result.IsLockedOut)
            {
                return Forbid();
            }
            else
            {
                // La idea es: si el usuario no tiene una cuenta creada y enlazada con el proveedor (hubiera pasado por result.Succeeded)
                // exijimos (si aplica) que se cree una cuenta.
                // Podemos coger los Claims que nos provee el servicio externo.
                var email = info.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (string.IsNullOrWhiteSpace(email)) return SignOut();

                var identityUser = new MyUser();
                await UserStore.SetUserNameAsync(identityUser, email, System.Threading.CancellationToken.None);
                await EmailStore.SetEmailAsync(identityUser, email, System.Threading.CancellationToken.None);

                var createResult = await UserManager.CreateAsync(identityUser);
                if (createResult.Succeeded)
                {
                    await SignInManager.SignInAsync(identityUser, isPersistent: false);
                    return Redirect("/");
                }
                else
                {
                    foreach(var error in createResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
        }

        [HttpPost("signout"), HttpGet("signout"), AllowAnonymous]
        public IActionResult SignOut()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

}