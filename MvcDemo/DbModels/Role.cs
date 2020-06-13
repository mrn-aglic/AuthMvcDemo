using System;
using System.Collections.Generic;

namespace MvcDemo.DbModels
{
    public partial class Role
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
