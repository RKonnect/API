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
    public class FoodController : ControllerBase
    {

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Food>> AddFood(FoodDto request, [FromServices] DataContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Food name cannot be empty.");
            }

            var foodExists = await context.Food.FirstOrDefaultAsync(f => f.Name == request.Name);
            if (foodExists != null)
            {
                return BadRequest("This food already exists.");
            }

            var food = new Food
            {
                Name = request.Name,
            };

            context.Food.Add(food);
            await context.SaveChangesAsync();

            return Ok(food);
        }

    }
}
