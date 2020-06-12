using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace MvcDemo.AuthenticationMiddleware.JwtService
{
    public class JwtGenerator
    {
        private readonly string _tokenName;
        private readonly string _secret;
        private readonly string _issuer;
        private readonly int _durationInMinutes;
        public JwtGenerator(JwtConfig jwtConfig)
        {
            _tokenName = jwtConfig.TokenName;
            _secret = jwtConfig.Secret;
            _issuer = jwtConfig.Issuer;
            _durationInMinutes = jwtConfig.DurationInMinutes;
        }
        
        public TokenWithClaimsPrincipal Generate(string id, string username, string email, string[] roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email)
            }.Concat(roles.Select(r => new Claim(ClaimTypes.Role, r)))
                .ToList();
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var notBefore = DateTime.UtcNow;
            var expiration = DateTime.UtcNow.AddMinutes(_durationInMinutes);

            return null;
        }
    }
}