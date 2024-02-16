using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("addAllergy")]
        public async Task<IActionResult> addAllergy(Food request)
        {
            int allergyId = request.Id;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var food = _context.Food.FirstOrDefault(f => f.Id == allergyId);

            if (userId != null && allergyId != 0)
            {
                try
                {
                    int userIdInt = int.Parse(userId);
                    var user = _context.Utilisateur.FirstOrDefault(u => u.Id == userIdInt);

                    if (user != null && food != null)
                    {

                        user.Allergy ??= new List<UserAllergy>();

                        // Check if the user already has the allergy
                        if (!user.Allergy.Any(a => a.FoodId == allergyId))
                        {
                            user.Allergy.Add(new UserAllergy
                            {
                                UserId = userIdInt,
                                User = user,
                                FoodId = allergyId,
                                Food = food,
                            });

                            await _context.SaveChangesAsync();
                                return Ok($"Allergy with ID {allergyId} added for user with ID {userId}");
                        }
                        else
                        {
                            return Conflict("User already has this allergy");
                        }
                    }
                    else
                    {
                        return NotFound("User or Food not found");
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
