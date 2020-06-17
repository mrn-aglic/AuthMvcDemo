using System;
using System.Collections.Generic;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;

namespace MvcDemo.DbModels
{
    public partial class UserRole : CsUserRole<Appuser, int, Role, int>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Appuser User { get; set; }
    }
}
