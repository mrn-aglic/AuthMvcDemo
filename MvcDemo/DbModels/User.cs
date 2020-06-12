using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;

namespace MvcDemo.DbModels
{
    public class User : CsUser<int>
    {
    }

    public class Role : CsRole<int>
    {
    }
}