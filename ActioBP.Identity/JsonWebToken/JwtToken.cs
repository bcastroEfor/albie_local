using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ActioBP.Identity.JsonWebToken
{
    public sealed class JwtToken
    {
        private JwtSecurityToken token;
        public DateTime ValidTo => token.ValidTo;
        public string AccessToken => new JwtSecurityTokenHandler().WriteToken(this.token);
        public string Username => token.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        public Dictionary<string, object> ExtraData = new Dictionary<string, object>();

        internal JwtToken(JwtSecurityToken token, Dictionary<string, object> extraData = null)
        {
            if (extraData != null) this.ExtraData = extraData;
            this.token = token;
        }
    }
}
