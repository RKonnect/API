using API_RKonnect.Dto;
using API_RKonnect.Models;
using API_RKonnect;
using Microsoft.AspNetCore.Mvc;

public interface ITagService
{
    IActionResult GetAll([FromServices] DataContext context);

    IActionResult GetById(int tagId, [FromServices] DataContext context);

    IActionResult GetByName(string tagName, [FromServices] DataContext context);

    Task<ActionResult<Tag>> AddTag(TagDto request, [FromServices] DataContext context);

    Task<ActionResult<Tag>> UpdateTag(int tagId, TagDto request, [FromServices] DataContext context);

    Task<ActionResult<Tag>> DeleteTag(int tagId, [FromServices] DataContext context);
}
