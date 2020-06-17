using System;
using System.Collections.Generic;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;

namespace MvcDemo.DbModels
{
    public partial class Appuser : CsUser<int>
    {
        public Appuser()
        {
            UserRole = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PasswordHash { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
