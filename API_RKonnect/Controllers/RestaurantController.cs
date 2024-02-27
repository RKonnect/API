using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly DataContext _context;

        public RestaurantController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("getAll")]
        public IActionResult getAll()
        {
            var restaurants = _context.Restaurant
                .Select(restaurants => new RestaurantDto
                {
                    Id = restaurants.Id,
                    Name = restaurants.Name,
                    Url = restaurants.Url,
                    Picture = restaurants.Picture,
                    Price = restaurants.Price,
                    VegetarianDish = restaurants.VegetarianDish,
                    User = new PublicUserDto
                    {
                        Id = restaurants.User.Id,
                        Pseudo = restaurants.User.Pseudo,
                        Avatar = restaurants.User.Avatar,
                        Email = restaurants.User.Email,
                        CreatedAt = restaurants.User.CreatedAt,
                        UpdatedAt = restaurants.User.UpdatedAt
                    }
                })
                .ToList();

            return Ok(restaurants);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Restaurant>> AddRestaurant(Restaurant request, [FromServices] DataContext context)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                try
                {
                    int userIdInt = int.Parse(userId);
                    var user = _context.Utilisateur.FirstOrDefault(u => u.Id == userIdInt);

                    if (user != null)
                    {
                        var restaurantExist = await context.Restaurant.FirstOrDefaultAsync(r => r.Name == request.Name);
                        if (restaurantExist != null)
                        {
                            return BadRequest("This restaurant already exists.");
                        }

                        var newRestaurant = new Restaurant
                        {
                            Name = request.Name,
                            Picture = request.Picture,
                            Price = request.Price,
                            UserId = userIdInt,
                            User = user
                        };

                        context.Restaurant.Add(newRestaurant);
                        await context.SaveChangesAsync();

                        return Ok($"The new restaurant {newRestaurant.Name} has been added");
                    }
                    else
                    {
                        return NotFound("User not found");
                    }
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid user ID format");
                }
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
