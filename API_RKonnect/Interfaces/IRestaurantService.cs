using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_RKonnect.Interfaces
{
    public interface IRestaurantService
    {
        IActionResult getAll([FromServices] DataContext context);
        IActionResult getById(int restaurantId, [FromServices] DataContext context);

        IActionResult getByUserId(int userId, [FromServices] DataContext context);

        Task<ActionResult<Restaurant>> AddRestaurant(RestaurantDto request, int userId, [FromServices] DataContext context);
        Task<IActionResult> Update(UpdateRestaurantDto request, int userId, int restaurantId, [FromServices] DataContext context);
    }
}
