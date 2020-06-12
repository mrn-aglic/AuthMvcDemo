using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MvcDemo.AuthenticationMiddleware.JwtService
{
    public class JwtConfig
    {
        public string TokenName { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int DurationInMinutes { get; set; }

        public SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
        }

        public TokenValidationParameters ToTokenValidationParameters() => 
            new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                
                ValidateAudience = false,
                
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                
                IssuerSigningKey = GetSecurityKey(),
                ValidateIssuerSigningKey = true,

                RequireExpirationTime = true,
                ValidateLifetime = true
            };
    }
}