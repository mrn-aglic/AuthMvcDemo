namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.Interfaces
{
    public interface IRole<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}