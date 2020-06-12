using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.Interfaces;

namespace MvcDemo.DbModels
{
    public class User : CsUser<int>
    {
    }

    public class Role : CsRole<int>
    {
    }
}