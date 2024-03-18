using API_RKonnect.Dto;
using API_RKonnect.Enums;
using API_RKonnect.Interfaces;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Security.Claims;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        [Authorize]
        [HttpGet("getAll")]
        public IActionResult getAll([FromServices] DataContext context, [FromServices] IRestaurantService restaurantService)
        {
            return restaurantService.getAll(context);
        }

        [Authorize]
        [HttpGet("getById/{restaurantId}")]
        public IActionResult getById(int restaurantId, [FromServices] DataContext context, [FromServices] IRestaurantService restaurantService)
        {
            return restaurantService.getById(restaurantId, context);
        }

        // Algo de recommandation en fonction des intérêts de l'utilisateur
        [Authorize]
        [HttpGet("getByUserId/{userId}")]
        public IActionResult getByUserId(int userId, [FromServices] DataContext context, [FromServices] IRestaurantService restaurantService)
        {
            return restaurantService.getByUserId(userId, context);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Restaurant>> AddRestaurant(RestaurantDto request, [FromServices] DataContext context, [FromServices] IRestaurantService restaurantService)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user);

            return await restaurantService.AddRestaurant(request, userId, context);
        }

        [Authorize]
        [HttpPut("update/{restaurantId}")]
        public async Task<IActionResult> Update(UpdateRestaurantDto request, int restaurantId, [FromServices] DataContext context, [FromServices] IRestaurantService restaurantService)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user);

            return await restaurantService.Update(request, userId, restaurantId, context);
        }
    }
}
