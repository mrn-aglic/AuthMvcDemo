namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.Interfaces
{
    public class CsUser<T>
    {
        public T Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        
        // public IEnumerable<>
    }
}