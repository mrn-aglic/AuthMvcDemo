using Microsoft.AspNetCore.Mvc;

namespace MvcDemo.Controllers
{
    public class CoursesController : Controller
    {
        // GET
        public IActionResult Courses()
        {
            return View();
        }
    }
}