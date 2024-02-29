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
        public async Task<ActionResult<Tag>> AddTag(TagDto request, [FromServices] DataContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("Tag title cannot be empty.");
            }

            var tagExists = await context.Tag.FirstOrDefaultAsync(t => t.Title == request.Title);
            if (tagExists != null)
            {
                return BadRequest("This tag already exists.");
            }

            var tag = new Tag
            {
                Title = request.Title,
            };

            context.Tag.Add(tag);
            await context.SaveChangesAsync();

            return Ok(tag);
        }

    }
}
