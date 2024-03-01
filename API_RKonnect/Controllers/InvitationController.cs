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
        [HttpGet("getById/{userId}")]
        public IActionResult GetInvitationById(int userId)
        {
            var currentUser = _context.Utilisateur.FirstOrDefault(u => u.Id == userId);
            if (currentUser != null)
            {
                 var invitations = _context.UserInvitation
                .Where(ui => ui.UserId == userId)
                .Include(ui => ui.Invitation)
                .Select(ui => new GetInvitationInfoDto
                {
                    InvitationId = ui.InvitationId,
                    IsAccepted = ui.IsAccepted,
                    Guests = _context.UserInvitation
                                .Where(u => u.InvitationId == ui.InvitationId && u.UserId != userId)
                                .Select(u => new PublicUserDto
                                {
                                    Id = u.UserId,
                                    Pseudo = u.User.Pseudo,
                                    Avatar = u.User.Avatar,
                                    Email = u.User.Email,
                                    CreatedAt = u.User.CreatedAt,
                                    UpdatedAt = u.User.UpdatedAt
                                }).ToList()
                }).ToList();

                if (invitations == null)
                {
                    return NotFound();
                }

                return Ok(invitations);
            }
            else
            {
                return BadRequest("User not found.");
            }
        }

        [Authorize]
        [HttpGet("join/{invitationId}")]
        public async Task<IActionResult> JoinInvitation(int invitationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var invitationExist = await _context.Invitation.FirstOrDefaultAsync(ie => ie.Id == invitationId);

            //check if places are available
            var invitationUserMax = invitationExist.UserMax;
            var countAcceptedInvitations = await _context.UserInvitation
                .Where(invitation => invitation.InvitationId == invitationId && invitation.IsAccepted == true)
                .CountAsync();

            if (userId != null || invitationExist != null)
            {
                int userIdInt = int.Parse(userId);
                var user = await _context.Utilisateur.FirstOrDefaultAsync(u => u.Id == userIdInt);
                bool reservationExists = _context.UserInvitation.Any(r => r.InvitationId == invitationId && r.UserId == userIdInt);

                if (user != null)
                {
                    if(reservationExists != true)
                    {
                        if (countAcceptedInvitations < invitationUserMax)
                        {
                            var newUserInvitation = new UserInvitation
                            {
                                InvitationId = invitationExist.Id,
                                Invitation = invitationExist,
                                UserId = userIdInt,
                                User = user,
                                IsAccepted = false
                            };
                            _context.UserInvitation.Add(newUserInvitation);
                            await _context.SaveChangesAsync();

                            return Ok("Join success !");
                        }
                        else
                        {
                            return BadRequest("There are no more places available.");
                        }
                    }
                    else
                    {
                        return BadRequest("Reservation already exists for this user.");
                    } 
                }
                else
                {
                    return BadRequest("User not found.");
                }
            }
            else
            {
                return BadRequest("User id or invitation not found.");
            }
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
                                Host = user,
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
