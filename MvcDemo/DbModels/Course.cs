using System;
using System.Collections.Generic;

namespace MvcDemo.DbModels
{
    public partial class Course
    {
        public Course()
        {
            StudentCourse = new HashSet<StudentCourse>();
        }

        public string Isvu { get; set; }
        public string Name { get; set; }
        public string Shortname { get; set; }
        public string Level { get; set; }
        public int Ects { get; set; }
        public int? DepartmentId { get; set; }
        public int Id { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<StudentCourse> StudentCourse { get; set; }
    }
}
