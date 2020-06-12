using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using MvcDemo.Models;
using MvcDemo.DtoMappers;

namespace MvcDemo.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesApiController : Controller
    {
        private List<Course> _courses;

        public CoursesApiController()
        {
            _courses = new List<Course>();
            _courses.Add(
                new Course(
                    0, "123", "Programiranje mrežnih aplikacija", "PMA", "Preddiplomski",
                    5, "Odjel za informatiku"
                )
            );
            _courses.Add(
                new Course(
                    1, "342", "Programiranje 2", "P2", "Preddiplomski",
                    5, "Odjel za informatiku"
                )
            );
            _courses.Add(new Course(
                    2, "456", "Primjenjena statistika", "PS", "Preddiplomski",
                    6, "Odjel za matematiku"
                )
            );
        }

        [HttpGet]
        public ActionResult<List<Course>> Get()
        {
            return _courses;
        }

        [HttpGet("{id}")] // api/courses/0
        public ActionResult<Course> Get(int id)
        {
            return _courses.Find(x => x.Id == id);
        }

        [HttpPost("savecourse")]
        public ActionResult Save(Course course)
        {
            _courses.Add(course);
            return Ok();
        }

        [HttpPost("savecourse2")]
        public ActionResult Save([FromBody] JObject json)
        {
            var course = CourseDto.FromJson(json);
            _courses.Add(course);
            return Ok();
        }
    }
}