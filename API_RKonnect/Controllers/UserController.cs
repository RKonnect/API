using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [Authorize]
        [HttpPost("addAllergy")]
        public async Task<IActionResult> addAllergy()
        {
            //TODO
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                return Ok($"User ID: {userId}, Authorized");
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
