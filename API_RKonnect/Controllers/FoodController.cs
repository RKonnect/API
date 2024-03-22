using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        [Authorize]
        [HttpGet("getAll")]
        public IActionResult GetAll([FromServices] DataContext context, [FromServices] IFoodServices foodServices)
        {
            return foodServices.GetAll(context);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<string>> AddFood(FoodDto request, [FromServices] DataContext context, [FromServices] IFoodServices foodServices)
        {
            return await foodServices.AddFood(request, context);
        }

    }
}
