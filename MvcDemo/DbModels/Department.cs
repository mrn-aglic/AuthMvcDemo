using System;
using System.Collections.Generic;

namespace MvcDemo.DbModels
{
    public partial class Department
    {
        public Department()
        {
            Course = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Shortname { get; set; }

        public virtual ICollection<Course> Course { get; set; }
    }
}
