namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses
{
    public interface CsUser<T>
    {
        public T Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        
        // public ICollection<U> UserRole { get; set; }

        // public IEnumerable<>
    }
}