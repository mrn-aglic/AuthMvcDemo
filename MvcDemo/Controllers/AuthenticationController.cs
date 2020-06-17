using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcDemo.AuthenticationMiddleware.CustomIdentityStores.BaseClasses;
using MvcDemo.AuthenticationMiddleware.JwtService;
using MvcDemo.DbModels;
using MvcDemo.Models.AuthModels;

namespace MvcDemo.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly JwtGenerator _jwtGenerator;
        private readonly UserManager<Appuser> _userManager;

        public AuthenticationController
        (
            JwtGenerator jwtGenerator,
            UserManager<Appuser> userManager
        )
        {
            _jwtGenerator = jwtGenerator;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromForm] RegistrationModel registrationModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Login), "Authentication");
            }

            var user = new Appuser
            {
                Firstname = registrationModel.Firstname,
                Lastname = registrationModel.Lastname,
                Username = registrationModel.Username,
                Email = registrationModel.Email,
                PasswordHash = registrationModel.Password
            };
            var result = await _userManager.CreateAsync(user);

            var accessTokenResult = _jwtGenerator.Generate(user.Id.ToString(), user.Username, user.Email,
                new[] {"Admin"});
            // var accessTokenResult = _jwtGenerator.Generate(
            // "0",
            // "test",
            // "test@test.com",
            // new[] {"Admin"}
            // );
            await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal,
                accessTokenResult.AuthProperties);
            return RedirectToAction(nameof(HomeController.Index), "Home");
            // return Ok();
        }
    }
}