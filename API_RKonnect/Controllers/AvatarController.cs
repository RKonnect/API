using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        [Authorize]
        [HttpGet("getAll")]
        public IActionResult GetAll([FromServices] DataContext context, IAvatarService avatarService)
        {
            return avatarService.GetAll(context);
        }
    }
}
