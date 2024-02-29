using API_RKonnect.Dto;
using API_RKonnect.Enums;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly DataContext _context;

        public InvitationController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Invitation>> AddInvitation(InvitationDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var restaurantExists = await _context.Restaurant.FirstOrDefaultAsync(r => r.Id == request.RestaurantId);

            if (userId != null)
            {
                int userIdInt = int.Parse(userId);
                var user = await _context.Utilisateur.FirstOrDefaultAsync(u => u.Id == userIdInt);

                if (restaurantExists != null)
                {
                    if (request.PaymentType.HasValue)
                    {
                        if (Enum.IsDefined(typeof(PaymentType), request.PaymentType.Value))
                        {
                            var newInvitation = new Invitation
                            {
                                Restaurant = restaurantExists,
                                UserMax = request.UserMax,
                                PaymentType = request.PaymentType,
                                InvitationDate = request.InvitationDate.ToUniversalTime()
                            };
                            _context.Invitation.Add(newInvitation);

                            var newUserInvitation = new UserInvitation
                            {
                                InvitationId = newInvitation.Id,
                                Invitation = newInvitation,
                                UserId = userIdInt,
                                User = user,
                                IsAccepted = true
                            };
                           _context.UserInvitation.Add(newUserInvitation);

                            await _context.SaveChangesAsync();

                            return Ok("The invitation has been saved.");
                        }
                        else
                        {
                            throw new ArgumentException("The specified payment type is invalid.");
                        }
                    }
                    else
                    {
                        return BadRequest("The payment type is not defined.");
                    }
                }
                else
                {
                    return BadRequest("This restaurant does not exist.");
                }
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
