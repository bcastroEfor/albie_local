using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Albie.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IConfiguration Config;

        public BaseController(IConfiguration configuration)
        {
            this.Config = configuration;
        }

        #region properties
        protected Guid UserId
        {
            get
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;

                var id = identity.Claims.SingleOrDefault(o => o.Type == "id");
                if (id == null) return Guid.Empty;
                return Guid.Parse(id.Value);
            }
        }

        protected string LangCode
        {
            get
            {
                var lang = "es";
                var header = Request.Headers.SingleOrDefault(h => h.Key.Equals("language", StringComparison.OrdinalIgnoreCase));
                lang = header.Value.FirstOrDefault();
                if (string.IsNullOrEmpty(lang)) lang = "es";
                return lang;
            }
        }
        #endregion

    }
}