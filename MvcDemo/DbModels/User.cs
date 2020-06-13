using System;
using System.Collections.Generic;

namespace MvcDemo.DbModels
{
    public partial class User
    {
        public User()
        {
            UserRole = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PasswordHash { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
