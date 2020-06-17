namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses
{
    public interface CsRole<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}