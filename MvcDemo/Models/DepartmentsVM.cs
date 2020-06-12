using System.Collections.Generic;

namespace MvcDemo.Models
{
    public class DepartmentsVm
    {
        public IEnumerable<DepartmentVm> Departments { get; set; }

        public DepartmentsVm(IEnumerable<DepartmentVm> departments)
        {
            Departments = departments;
        }
    }
}