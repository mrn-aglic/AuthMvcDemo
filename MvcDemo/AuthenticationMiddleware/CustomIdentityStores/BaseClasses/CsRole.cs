namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses
{
    public class CsRole<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}