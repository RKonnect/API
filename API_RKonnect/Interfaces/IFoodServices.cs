using API_RKonnect;
using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Mvc;

public interface IFoodServices
{
    Task<ActionResult<string>> AddFood(FoodDto request, [FromServices] DataContext context);
}
