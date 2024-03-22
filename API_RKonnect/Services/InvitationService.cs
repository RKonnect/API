using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API_RKonnect.Dto;
using API_RKonnect.Models;
using API_RKonnect.Enums;
using System.Collections.Generic;

namespace API_RKonnect.Services
{
    public class InvitationService : IInvitationService
    {
        public IActionResult getAll([FromServices] DataContext context)
        {
            var invitations = context.Invitation
                .Select(invitation => new GetInvitationInfoDto
                {
                    InvitationId = invitation.Id,
                    Guests = context.UserInvitation
                                    .Where(u => u.InvitationId == invitation.Id)
                                    .Select(u => new PublicUserDto
                                    {
                                        Id = u.UserId,
                                        Pseudo = u.User.Pseudo,
                                        Avatar = u.User.Avatar,
                                        Email = u.User.Email,
                                        CreatedAt = u.User.CreatedAt,
                                        UpdatedAt = u.User.UpdatedAt
                                    }).ToList(),
                    Host = new PublicUserDto
                    {
                        Id = invitation.Host.Id,
                        Pseudo = invitation.Host.Pseudo,
                        Avatar = invitation.Host.Avatar,
                        Email = invitation.Host.Email,
                        CreatedAt = invitation.Host.CreatedAt,
                        UpdatedAt = invitation.Host.UpdatedAt
                    },
                    Restaurants = new RestaurantDto
                    {
                        Id = invitation.Restaurant.Id,
                        Name = invitation.Restaurant.Name,
                        Url = invitation.Restaurant.Url,
                        Picture = invitation.Restaurant.Picture,
                        Price = invitation.Restaurant.Price,
                        VegetarianDish = invitation.Restaurant.VegetarianDish,
                        User = new PublicUserDto
                        {
                            Id = invitation.Host.Id,
                            Pseudo = invitation.Host.Pseudo,
                            Avatar = invitation.Host.Avatar,
                            Email = invitation.Host.Email,
                            CreatedAt = invitation.Host.CreatedAt,
                            UpdatedAt = invitation.Host.UpdatedAt
                        },
                        Localisation = new Localisation
                        {
                            Id = invitation.Restaurant.Localisation.Id,
                            Lat = invitation.Restaurant.Localisation.Lat,
                            Lng = invitation.Restaurant.Localisation.Lng,
                            Adress = invitation.Restaurant.Localisation.Adress,
                            City = invitation.Restaurant.Localisation.City,
                            ZipCode = invitation.Restaurant.Localisation.ZipCode
                        }
                    }
                }).ToList();

            return new OkObjectResult(invitations);
        }


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
                                    }).ToList(),
                        Host = new PublicUserDto
                        {
                            Id = currentUser.Id,
                            Pseudo = currentUser.Pseudo,
                            Avatar = currentUser.Avatar,
                            Email = currentUser.Email,
                            CreatedAt = currentUser.CreatedAt,
                            UpdatedAt = currentUser.UpdatedAt
                        },
                        Restaurants = context.Invitation
                                        .Where(i => i.Id == ui.InvitationId)
                                        .Select(i => new RestaurantDto
                                        {
                                            Id = i.Restaurant.Id,
                                            Name = i.Restaurant.Name,
                                            Url = i.Restaurant.Url,
                                            Picture = i.Restaurant.Picture,
                                            Price = i.Restaurant.Price,
                                            VegetarianDish = i.Restaurant.VegetarianDish,
                                            User = new PublicUserDto
                                            {
                                                Id = i.Host.Id,
                                                Pseudo = i.Host.Pseudo,
                                                Avatar = i.Host.Avatar,
                                                Email = i.Host.Email,
                                                CreatedAt = i.Host.CreatedAt,
                                                UpdatedAt = i.Host.UpdatedAt
                                            },
                                            Localisation = new Localisation
                                            {
                                                Id = i.Restaurant.Localisation.Id,
                                                Lat = i.Restaurant.Localisation.Lat,
                                                Lng = i.Restaurant.Localisation.Lng,
                                                Adress = i.Restaurant.Localisation.Adress,
                                                City = i.Restaurant.Localisation.City,
                                                ZipCode = i.Restaurant.Localisation.ZipCode
                                            }
                                        }).FirstOrDefault()
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
