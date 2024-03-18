using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace API_RKonnect.Services
{
    public class TagService : ITagService
    {
        public async Task<ActionResult<Tag>> AddTag(TagDto request, [FromServices] DataContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new BadRequestObjectResult("Tag title cannot be empty.");
            }

            var tagExists = await context.Tag.FirstOrDefaultAsync(t => t.Title == request.Title);
            if (tagExists != null)
            {
                return new BadRequestObjectResult("This tag already exists.");
            }

            var tag = new Tag
            {
                Title = request.Title,
            };

            context.Tag.Add(tag);
            await context.SaveChangesAsync();

            return new OkObjectResult(tag);
        }
    }
}
