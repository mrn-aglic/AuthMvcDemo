using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;

namespace MvcDemo.AuthenticationMiddleware.JwtService
{
    public class JwtAuthTicketFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IDataSerializer<AuthenticationTicket> _ticketSerializer;
        private readonly IDataProtector _dataProtector;

        public JwtAuthTicketFormat
        (
            JwtOptions jwtOptions,
            IDataSerializer<AuthenticationTicket> ticketSerializer,
            IDataProtector dataProtector
        )
        {
            _jwtOptions = jwtOptions;
            _ticketSerializer = ticketSerializer;
            _dataProtector = dataProtector;
        }

        public static JwtAuthTicketFormat Create(JwtConfig jwtConfig,
            IDataSerializer<AuthenticationTicket> ticketSerializer,
            IDataProtector dataProtector)
        {
            return new JwtAuthTicketFormat(
                new JwtOptions(jwtConfig.TokenName, jwtConfig),
                ticketSerializer, 
                dataProtector);
        }

        public string Protect(AuthenticationTicket data) => Protect(data, null);

        public string Protect(AuthenticationTicket data, string purpose)
        {
            var array = _ticketSerializer.Serialize(data);

            return Base64UrlTextEncoder.Encode(_dataProtector.Protect(array));
        }

        public AuthenticationTicket Unprotect(string protectedText) => Unprotect(protectedText, null);

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var authTicket = default(AuthenticationTicket);
            var algorithm = JwtOptions.Algorithm;

            try
            {
                authTicket = _ticketSerializer.Deserialize(
                    _dataProtector.Unprotect(
                        Base64UrlTextEncoder.Decode(protectedText)));

                var embeddedJwt = authTicket
                    .Properties?
                    .GetTokenValue(_jwtOptions.TokenName);
                new JwtSecurityTokenHandler()
                    .ValidateToken(embeddedJwt, _jwtOptions.ValidationParameters, out var token);

                if (!(token is JwtSecurityToken jwt))
                {
                    throw new SecurityTokenValidationException("JWT token was found to be invalid");
                }

                if (!jwt.Header.Alg.Equals(algorithm, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Algorithm must be '{algorithm}'");
                }
            }
            catch (Exception)
            {
                return null;
            }

            return authTicket;
        }
    }
}