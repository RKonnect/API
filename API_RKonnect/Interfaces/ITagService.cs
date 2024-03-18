using API_RKonnect.Dto;
using API_RKonnect.Models;
using API_RKonnect;
using Microsoft.AspNetCore.Mvc;

public interface ITagService
{
    Task<ActionResult<Tag>> AddTag(TagDto request, [FromServices] DataContext context);
}
