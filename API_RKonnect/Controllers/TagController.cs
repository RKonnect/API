using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Tag>> AddTag(TagDto request, [FromServices] DataContext context, [FromServices] ITagService tagService)
        {
            return await tagService.AddTag(request, context);
        }

    }
}
