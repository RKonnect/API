using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API_RKonnect.Dto;
using API_RKonnect.Models;
using API_RKonnect;

public class FoodServices : IFoodServices
{
    public async Task<ActionResult<string>> AddFood(FoodDto request, [FromServices] DataContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Icon))
        {
            return new BadRequestObjectResult("Food name or icon cannot be empty.");
        }

        var foodExists = await context.Food.FirstOrDefaultAsync(f => f.Name == request.Name);
        if (foodExists != null)
        {
            return new BadRequestObjectResult("This food already exists.");
        }

        var food = new Food
        {
            Name = request.Name,
            Icon = request.Icon
        };

        context.Food.Add(food);
        await context.SaveChangesAsync();

        return new OkObjectResult($"The food {food.Name} has been registered ");
    }
}
