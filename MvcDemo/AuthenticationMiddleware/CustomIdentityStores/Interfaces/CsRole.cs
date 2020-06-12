namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.Interfaces
{
    public class CsRole<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}