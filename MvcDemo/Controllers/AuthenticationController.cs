using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcDemo.AuthenticationMiddleware.JwtService;

namespace MvcDemo.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly JwtGenerator _jwtGenerator;

        public AuthenticationController(JwtGenerator jwtGenerator)
        {
            _jwtGenerator = jwtGenerator;
        }

        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> Register()
        {
            var accessTokenResult = _jwtGenerator.Generate(
                "0",
                "test",
                "test@test.com",
                new[] {"Admin"}
            );
            await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal,
                accessTokenResult.AuthProperties);
            return RedirectToAction(nameof(HomeController.Index), "Home");
            // return Ok();
        }
    }
}