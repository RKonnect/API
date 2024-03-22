using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {

        [Authorize]
        [HttpGet("getAll")]
        public IActionResult GetAll([FromServices] DataContext context, ITagService tagService)
        {
            return tagService.GetAll(context);
        }

        [Authorize]
        [HttpGet("getById/{tagId}")]
        public IActionResult GetById(int tagId, [FromServices] DataContext context, ITagService tagService)
        {
            return tagService.GetById(tagId, context);
        }

        [Authorize]
        [HttpGet("getByName/{tagName}")]
        public IActionResult GetByName(string tagName, [FromServices] DataContext context, ITagService tagService)
        {
            return tagService.GetByName(tagName, context);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Tag>> AddTag(TagDto request, [FromServices] DataContext context, [FromServices] ITagService tagService)
        {
            return await tagService.AddTag(request, context);
        }

        [Authorize]
        [HttpPut("update/{tagId}")]
        public async Task<ActionResult<Tag>> UpdateTag(int tagId, TagDto request, [FromServices] DataContext context, [FromServices] ITagService tagService)
        {
            return await tagService.UpdateTag(tagId, request, context);
        }

        [Authorize]
        [HttpDelete("delete/{tagId}")]
        public async Task<ActionResult<Tag>> DeleteTag(int tagId, [FromServices] DataContext context, [FromServices] ITagService tagService)
        {
            return await tagService.DeleteTag(tagId, context);
        }

    }
}
