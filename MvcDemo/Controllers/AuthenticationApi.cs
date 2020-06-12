using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MvcDemo.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationApi : Controller
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register()
        {
            
            return Ok();
        }
    }
}