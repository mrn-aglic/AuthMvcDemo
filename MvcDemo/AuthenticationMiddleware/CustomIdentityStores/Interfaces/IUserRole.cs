namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.Interfaces
{
    public interface IUserRole<T, U>
    {
        public T UserId { get; set; }
        public U RoleId { get; set; }
        
        public IUser<T> User { get; set; }
        public IRole<U> Role { get; set; }
    }
}