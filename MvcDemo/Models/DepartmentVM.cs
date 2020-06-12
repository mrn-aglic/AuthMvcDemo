using System.Collections.Generic;

namespace MvcDemo.Models
{
    public class DepartmentVm
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public IEnumerable<string> Programs { get; set; }

        public DepartmentVm(string name, string shortName, IEnumerable<string> programs)
        {
            Name = name;
            ShortName = shortName;
            Programs = programs;
        }
    }
}