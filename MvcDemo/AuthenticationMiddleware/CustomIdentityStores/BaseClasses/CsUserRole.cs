namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses
{
    public interface CsUserRole<T, TType, U, UType> where T : class, CsUser<TType> where U : class, CsRole<UType>
    {
        public TType UserId { get; set; }
        public UType RoleId { get; set; }

        public virtual T CsUser
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public virtual U CsRole
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}