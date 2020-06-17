using System;
using System.Collections.Generic;

namespace MvcDemo.DbModels
{
    public partial class Student
    {
        public Student()
        {
            StudentCourse = new HashSet<StudentCourse>();
        }

        public int Id { get; set; }
        public string Jmbag { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<StudentCourse> StudentCourse { get; set; }
    }
}
