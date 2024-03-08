using API_RKonnect.Models;
using Microsoft.AspNetCore.Mvc;
using API_RKonnect.Dto;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(AuthDto request, [FromServices] IAuthenticationService authenticationService, [FromServices] DataContext context)
        {
            return await authenticationService.Register(request, context);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(AuthDto request, [FromServices] IAuthenticationService authenticationService, [FromServices] DataContext context)
        {
            return await authenticationService.Login(request, context);
        }
    }
}
