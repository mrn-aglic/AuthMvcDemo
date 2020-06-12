using Newtonsoft.Json.Linq;
using MvcDemo.Models;

namespace MvcDemo.DtoMappers
{
    public static class CourseDto
    {
        public static Course FromJson(JObject json)
        {
            var name = json["name"].ToObject<string>();
            var shortName = json["shortname"].ToObject<string>();
            var isvu = json["isvu"].ToObject<string>();
            var ects = json["ects"].ToObject<int>();
            var department = json["department"].ToObject<string>();
            var level = json["level"].ToObject<string>();
            
            return new Course(-1, isvu, name, shortName, level, ects, department);
        }
    }
}