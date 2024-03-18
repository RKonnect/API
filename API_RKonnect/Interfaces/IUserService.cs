using API_RKonnect;
using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Mvc;

public interface IUserService
{
    IActionResult GetAll([FromServices] DataContext context);
    IActionResult GetById(int userId, [FromServices] DataContext context);
    Task<IActionResult> UpdateUser(UpdateUserDto request, int userId, [FromServices] DataContext context);
    Task<IActionResult> addAllergy(int allergyId, int userId, [FromServices] DataContext context);
    Task<IActionResult> addFavorite(int favoriteId, int userId, [FromServices] DataContext context);
    Task<IActionResult> addTag(int tagId, int userId, [FromServices] DataContext context);
}

