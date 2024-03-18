using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API_RKonnect.Dto;
using API_RKonnect.Models;
using API_RKonnect.Enums;

namespace API_RKonnect.Services
{
    public class InvitationService : IInvitationService
    {
        public IActionResult GetInvitationById(int userId, [FromServices] DataContext context)
        {
            var currentUser = context.Utilisateur.FirstOrDefault(u => u.Id == userId);
            if (currentUser != null)
            {
                var invitations = context.UserInvitation
               .Where(ui => ui.UserId == userId)
               .Include(ui => ui.Invitation)
               .Select(ui => new GetInvitationInfoDto
               {
                   InvitationId = ui.InvitationId,
                   IsAccepted = ui.IsAccepted,
                   Guests = context.UserInvitation
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
                    return new NotFoundObjectResult(404);
                }

                return new OkObjectResult(invitations);
            }
            else
            {
                return new BadRequestObjectResult("User not found.");
            }
        }

        public async Task<IActionResult> JoinInvitation(int invitationId, int userId, [FromServices] DataContext context)
        {
            var invitationExist = await context.Invitation.FirstOrDefaultAsync(ie => ie.Id == invitationId);

            //check if places are available
            var invitationUserMax = invitationExist.UserMax;
            var countAcceptedInvitations = await context.UserInvitation
                .Where(invitation => invitation.InvitationId == invitationId && invitation.IsAccepted == true)
                .CountAsync();

            if (userId != null || invitationExist != null)
            {
                var user = await context.Utilisateur.FirstOrDefaultAsync(u => u.Id == userId);
                bool reservationExists = context.UserInvitation.Any(r => r.InvitationId == invitationId && r.UserId == userId);

                if (user != null)
                {
                    if (reservationExists != true)
                    {
                        if (countAcceptedInvitations < invitationUserMax)
                        {
                            var newUserInvitation = new UserInvitation
                            {
                                InvitationId = invitationExist.Id,
                                Invitation = invitationExist,
                                UserId = userId,
                                User = user,
                                IsAccepted = false
                            };
                            context.UserInvitation.Add(newUserInvitation);
                            await context.SaveChangesAsync();

                            return new OkObjectResult("Join success !");
                        }
                        else
                        {
                            return new BadRequestObjectResult("There are no more places available.");
                        }
                    }
                    else
                    {
                        return new BadRequestObjectResult("Reservation already exists for this user.");
                    }
                }
                else
                {
                    return new BadRequestObjectResult("User not found.");
                }
            }
            else
            {
                return new BadRequestObjectResult("User id or invitation not found.");
            }
        }

        public async Task<ActionResult<Invitation>> AddInvitation(InvitationDto request, int userId, [FromServices] DataContext context)
        {
            var restaurantExists = await context.Restaurant.FirstOrDefaultAsync(r => r.Id == request.RestaurantId);

            if (userId != null)
            {
                var user = await context.Utilisateur.FirstOrDefaultAsync(u => u.Id == userId);

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
                            context.Invitation.Add(newInvitation);

                            var newUserInvitation = new UserInvitation
                            {
                                InvitationId = newInvitation.Id,
                                Invitation = newInvitation,
                                UserId = userId,
                                User = user,
                                IsAccepted = true
                            };
                            context.UserInvitation.Add(newUserInvitation);

                            await context.SaveChangesAsync();

                            return new OkObjectResult("The invitation has been saved.");
                        }
                        else
                        {
                            throw new ArgumentException("The specified payment type is invalid.");
                        }
                    }
                    else
                    {
                        return new BadRequestObjectResult("The payment type is not defined.");
                    }
                }
                else
                {
                    return new BadRequestObjectResult("This restaurant does not exist.");
                }
            }
            else
            {
                return new UnauthorizedObjectResult(401);
            }
        }

    }
}
