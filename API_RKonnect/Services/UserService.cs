using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using API_RKonnect.Enums;
using System.Security.Claims;

namespace API_RKonnect.Services
{
    public class UserService : IUserService
    {
        public IActionResult GetAll([FromServices] DataContext context)
        {
            var users = context.Utilisateur
                .Select(user => new GetUserInfoDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Pseudo = user.Pseudo,
                    Email = user.Email,
                    Biography = user.Biography,
                    Avatar = user.Avatar,
                    Gender = user.Gender,
                    Role = user.Role,
                    Tags = user.UserTag.Select(ut => new TagDto { Title = ut.Tag.Title }).ToList(),
                    Allergy = user.Allergy.Select(ua => new FoodDto { Name = ua.Food.Name, Icon = ua.Food.Icon }).ToList(),
                    FavoriteFood = user.FavoriteFood.Select(ua => new FoodDto { Name = ua.Food.Name, Icon = ua.Food.Icon }).ToList(),
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                })
                .ToList();

            return new OkObjectResult(users);
        }

        public IActionResult GetById(int userId, [FromServices] DataContext context)
        {
            var selectedUser = context.Utilisateur
                .Where(u => u.Id == userId)
                .Select(user => new GetUserInfoDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Pseudo = user.Pseudo,
                    Email = user.Email,
                    Biography = user.Biography,
                    Avatar = user.Avatar,
                    Gender = user.Gender,
                    Role = user.Role,
                    Tags = user.UserTag.Select(ut => new TagDto { Title = ut.Tag.Title }).ToList(),
                    Allergy = user.Allergy.Select(ua => new FoodDto { Name = ua.Food.Name, Icon = ua.Food.Icon }).ToList(),
                    FavoriteFood = user.FavoriteFood.Select(ua => new FoodDto { Name = ua.Food.Name, Icon = ua.Food.Icon }).ToList(),
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                })
                .SingleOrDefault();

            if (selectedUser == null)
            {
                return new NotFoundObjectResult(404);
            }

            return new OkObjectResult(selectedUser);
        }

        public async Task<IActionResult> UpdateUser(UpdateUserDto request, int userId, [FromServices] DataContext context)
        {
            if (userId != null)
            {
                try
                {
                    var user = context.Utilisateur.FirstOrDefault(u => u.Id == userId);

                    if (user != null)
                    {
                        if (request.Name != null)
                            user.Name = request.Name;

                        if (request.Surname != null)
                            user.Surname = request.Surname;

                        if (request.Pseudo != null)
                            user.Pseudo = request.Pseudo;

                        if (request.Biography != null)
                            user.Biography = request.Biography;

                        if (request.Avatar != null)
                            user.Avatar = request.Avatar;

                        if (request.Gender.HasValue)
                        {
                            if (Enum.IsDefined(typeof(UserGender), request.Gender.Value))
                            {
                                user.Gender = request.Gender;
                            }
                            else
                            {
                                throw new ArgumentException("The specified gender is invalid.");
                            }
                        }

                        if (request.Role.HasValue)
                        {
                            if (Enum.IsDefined(typeof(UserRole), request.Role.Value))
                            {
                                user.Role = request.Role;
                            }
                            else
                            {
                                throw new ArgumentException("The specified gender is invalid.");
                            }
                        }

                        await context.SaveChangesAsync();
                        return new OkObjectResult($"User information updated successfully");
                    }
                    else
                    {
                        return new NotFoundObjectResult("User not found");
                    }
                }
                catch (FormatException)
                {
                    return new BadRequestObjectResult("Invalid user ID format");
                }
            }
            else
            {
                return new UnauthorizedObjectResult(401);
            }
        }

        public async Task<IActionResult> addAllergy(int allergyId, int userId, [FromServices] DataContext context)
        {
            var food = context.Food.FirstOrDefault(f => f.Id == allergyId);

            if (userId != null && allergyId != 0)
            {
                try
                {
                    var user = context.Utilisateur.FirstOrDefault(u => u.Id == userId);

                    if (user != null && food != null)
                    {

                        user.Allergy ??= new List<UserAllergy>();

                        // Check if the user already has the allergy
                        if (!user.Allergy.Any(a => a.FoodId == allergyId))
                        {
                            user.Allergy.Add(new UserAllergy
                            {
                                UserId = userId,
                                User = user,
                                FoodId = allergyId,
                                Food = food,
                            });

                            await context.SaveChangesAsync();
                            return new OkObjectResult($"Allergy with ID {allergyId} added for user with ID {userId}");
                        }
                        else
                        {
                            return new ConflictObjectResult("User already has this allergy");
                        }
                    }
                    else
                    {
                        return new NotFoundObjectResult("User or Food not found");
                    }
                }
                catch (FormatException)
                {
                    return new BadRequestObjectResult("Invalid user ID format");
                }
            }
            else
            {
                return new UnauthorizedObjectResult(401);
            }
        }

        public async Task<IActionResult> addFavorite(int favoriteId, int userId, [FromServices] DataContext context)
        {
            var food = context.Food.FirstOrDefault(f => f.Id == favoriteId);

            if (userId != null && favoriteId != 0)
            {
                try
                {
                    var user = context.Utilisateur.FirstOrDefault(u => u.Id == userId);

                    if (user != null && food != null)
                    {

                        user.FavoriteFood ??= new List<FavoriteFood>();

                        // Check if the user already has the favorite food
                        if (!user.FavoriteFood.Any(a => a.FoodId == favoriteId))
                        {
                            user.FavoriteFood.Add(new FavoriteFood
                            {
                                UserId = userId,
                                User = user,
                                FoodId = favoriteId,
                                Food = food,
                            });

                            await context.SaveChangesAsync();
                            return new OkObjectResult($"Food with ID {favoriteId} added for user with ID {userId}");
                        }
                        else
                        {
                            return new ConflictObjectResult("User already has this favorite food");
                        }
                    }
                    else
                    {
                        return new NotFoundObjectResult("User or Food not found");
                    }
                }
                catch (FormatException)
                {
                    return new BadRequestObjectResult("Invalid user ID format");
                }
            }
            else
            {
                return new UnauthorizedObjectResult(401);
            }
        }

        public async Task<IActionResult> addTag(int tagId, int userId, [FromServices] DataContext context)
        {
            var tag = context.Tag.FirstOrDefault(t => t.Id == tagId);

            if (userId != null && tagId != 0)
            {
                try
                {
                    var user = context.Utilisateur.FirstOrDefault(u => u.Id == userId);

                    if (user != null && tag != null)
                    {

                        user.UserTag ??= new List<UserTag>();

                        // Check if the user already has the favorite food
                        if (!user.UserTag.Any(t => t.TagId == tagId))
                        {
                            user.UserTag.Add(new UserTag
                            {
                                UserId = userId,
                                User = user,
                                TagId = tagId,
                                Tag = tag,
                            });

                            await context.SaveChangesAsync();
                            return new OkObjectResult($"Tag with ID {tagId} added for user with ID {userId}");
                        }
                        else
                        {
                            return new ConflictObjectResult("User already has this tag");
                        }
                    }
                    else
                    {
                        return new NotFoundObjectResult("User or Tag not found");
                    }
                }
                catch (FormatException)
                {
                    return new BadRequestObjectResult("Invalid user ID format");
                }
            }
            else
            {
                return new UnauthorizedObjectResult(401);
            }
        }

    }
}
