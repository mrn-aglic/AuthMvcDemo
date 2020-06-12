using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.Interfaces;
using MvcDemo.AuthenticationMiddleware.JwtService;
using MvcDemo.DbModels;

namespace MvcDemo.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly JwtGenerator _jwtGenerator;
        private readonly UserManager<CsUser<int>> _userManager;

        public AuthenticationController(JwtGenerator jwtGenerator, UserManager<CsUser<int>> userManager)
        {
            _jwtGenerator = jwtGenerator;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register()
        {
            var user = await _userManager.CreateAsync(new User
            {
                Email = "test@test.com",
                Username = "test",
                PasswordHash = "password"
            });

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