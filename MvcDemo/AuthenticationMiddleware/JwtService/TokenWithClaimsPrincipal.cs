using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace MvcDemo.AuthenticationMiddleware.JwtService
{
    public class TokenWithClaimsPrincipal
    {
        public string Token { get; }
        public ClaimsPrincipal ClaimsPrincipal { get; }
        public AuthenticationProperties AuthProperties { get; }

        public TokenWithClaimsPrincipal(string token, ClaimsPrincipal claimsPrincipal,
            AuthenticationProperties authProperties)
        {
            Token = token;
            ClaimsPrincipal = claimsPrincipal;
            AuthProperties = authProperties;
        }
    }
}