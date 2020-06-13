namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses
{
    public class CsUser<T>
    {
        public virtual T Id { get; set; }
        public virtual string Email { get; set; }
        public virtual string Username { get; set; }
        public virtual string PasswordHash { get; set; }
        
        // public IEnumerable<>
    }
}