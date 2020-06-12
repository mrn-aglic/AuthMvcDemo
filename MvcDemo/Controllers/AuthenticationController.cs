using Microsoft.AspNetCore.Mvc;

namespace MvcDemo.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET
        public IActionResult Login()
        {
            return View();
        }
    }
}