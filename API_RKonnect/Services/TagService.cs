using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace API_RKonnect.Services
{
    public class TagService : ITagService
    {
        public IActionResult GetAll([FromServices] DataContext context)
        {
            var tags = context.Tag
                .AsNoTracking()
                .ToList();

            return new OkObjectResult(tags);
        }

        public IActionResult GetById(int tagId, [FromServices] DataContext context)
        {
            var selectedTag = context.Tag
                 .Where(t => t.Id == tagId)
                 .Select(t => new
                 {
                     t.Id,
                     t.Title,
                 })
                 .FirstOrDefault();

            if (selectedTag == null)
            {
                return new NotFoundObjectResult(404);
            }

            return new OkObjectResult(selectedTag);
        }

        public IActionResult GetByName(string tagName, [FromServices] DataContext context)
        {
            var selectedTags = context.Tag
                .Where(t => t.Title.Contains(tagName))
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                })
                .ToList();

            if (selectedTags.Count == 0)
            {
                return new NotFoundObjectResult(404);
            }

            return new OkObjectResult(selectedTags);
        }

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

        public async Task<ActionResult<Tag>> UpdateTag(int tagId, TagDto request, [FromServices] DataContext context)
        {
            var selectTag = context.Tag.FirstOrDefault(t => t.Id == tagId);
            if (selectTag == null)
            {
                return new NotFoundObjectResult(404);
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new BadRequestObjectResult("Tag title cannot be empty.");
            }

            var tagExists = await context.Tag.FirstOrDefaultAsync(t => t.Title == request.Title);

            if (tagExists != null)
            {
                return new BadRequestObjectResult("This tag already exists.");
            }

            selectTag.Title = request.Title;

            context.Tag.Update(selectTag);
            await context.SaveChangesAsync();

            return new OkObjectResult(selectTag);
        }

        public async Task<ActionResult<Tag>> DeleteTag(int tagId, [FromServices] DataContext context)
        {
            var selectTag = context.Tag.FirstOrDefault(t => t.Id == tagId);
            if (selectTag == null)
            {
                return new NotFoundObjectResult(404);
            }

            context.Tag.Remove(selectTag);
            await context.SaveChangesAsync();

            return new OkObjectResult("Tag is deleted");
        }
    }
}
