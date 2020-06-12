using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcDemo.AuthenticationMiddleware.JwtService;

namespace MvcDemo.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationApiController : Controller
    {
        private readonly JwtGenerator _jwtGenerator;

        public AuthenticationApiController(JwtGenerator jwtGenerator)
        {
            _jwtGenerator = jwtGenerator;
        }

        [HttpPost("register")]
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