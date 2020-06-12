using Microsoft.IdentityModel.Tokens;

namespace MvcDemo.AuthenticationMiddleware.JwtService
{
    public class JwtOptions
    {
        public string TokenName { get; }
        public TokenValidationParameters ValidationParameters { get; }
        public static string Algorithm = SecurityAlgorithms.HmacSha256;

        public JwtOptions(string tokenName, JwtConfig jwtConfig)
        {
            ValidationParameters = jwtConfig.ToTokenValidationParameters();
            TokenName = tokenName;
        }
    }
}