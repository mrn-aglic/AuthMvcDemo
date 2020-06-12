namespace MvcDemo.AuthenticationMiddleware.JwtService
{
    public class JwtConfig
    {
        public string TokenName { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int DurationInMinutes { get; set; }
    }
}