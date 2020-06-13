namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses
{
    public class CsUserRole<T, U>
    {
        public T UserId { get; set; }
        public U RoleId { get; set; }
        
        public CsUser<T> CsUser { get; set; }
        public CsRole<U> CsRole { get; set; }
    }
}