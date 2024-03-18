using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API_RKonnect.Dto;
using API_RKonnect.Models;
using API_RKonnect.Enums;
using API_RKonnect;
using API_RKonnect.Interfaces;
using System.Security.Claims;

public class RestaurantService : IRestaurantService
{
    public IActionResult getAll([FromServices] DataContext context)
    {
        var restaurants = context.Restaurant
            .Select(restaurants => new RestaurantDto
            {
                Id = restaurants.Id,
                Name = restaurants.Name,
                Url = restaurants.Url,
                Picture = restaurants.Picture,
                Price = restaurants.Price,
                VegetarianDish = restaurants.VegetarianDish,
                User = new PublicUserDto
                {
                    Id = restaurants.User.Id,
                    Pseudo = restaurants.User.Pseudo,
                    Avatar = restaurants.User.Avatar,
                    Email = restaurants.User.Email,
                    CreatedAt = restaurants.User.CreatedAt,
                    UpdatedAt = restaurants.User.UpdatedAt
                },
                Localisation = new Localisation
                {
                    Id = restaurants.Localisation.Id,
                    Lat = restaurants.Localisation.Lat,
                    Lng = restaurants.Localisation.Lng,
                    Adress = restaurants.Localisation.Adress,
                    City = restaurants.Localisation.City,
                    ZipCode = restaurants.Localisation.ZipCode
                }
            })
            .ToList();

        return new OkObjectResult(restaurants);
    }

    public IActionResult getById(int restaurantId, [FromServices] DataContext context)
    {
        var SelectedRestaurant = context.Restaurant
            .Where(r => r.Id == restaurantId)
            .Select(restaurant => new RestaurantDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Url = restaurant.Url,
                Picture = restaurant.Picture,
                Price = restaurant.Price,
                VegetarianDish = restaurant.VegetarianDish,
                User = new PublicUserDto
                {
                    Id = restaurant.User.Id,
                    Pseudo = restaurant.User.Pseudo,
                    Avatar = restaurant.User.Avatar,
                    Email = restaurant.User.Email,
                    CreatedAt = restaurant.User.CreatedAt,
                    UpdatedAt = restaurant.User.UpdatedAt
                },
                Localisation = new Localisation
                {
                    Id = restaurant.Localisation.Id,
                    Lat = restaurant.Localisation.Lat,
                    Lng = restaurant.Localisation.Lng,
                    Adress = restaurant.Localisation.Adress,
                    City = restaurant.Localisation.City,
                    ZipCode = restaurant.Localisation.ZipCode
                }
            })
            .SingleOrDefault();

        if (SelectedRestaurant == null)
        {
            return new NotFoundObjectResult(404);
        }

        return new OkObjectResult(SelectedRestaurant);
    }

    public IActionResult getByUserId(int userId, [FromServices] DataContext context)
    {
        var user = context.Utilisateur.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return new NotFoundObjectResult("User not found");
        }

        // Récupérer les plats (food) favoris de l'utilisateur
        var favoritesFoodUserConnected = context.FavoriteFood
             .Where(f => f.UserId == userId)
             .Select(f => f.FoodId)
             .ToList();
        // Récupérer les tags de l'utilisateur
        var tagsUserConnected = context.UserTag
            .Where(t => t.UserId == userId)
            .Select(t => t.TagId)
            .ToList();

        // Comparer les plats favoris de l'utilisateur avec les autres utilisateurs
        // qui ont déjà accepté ou envoyé des invitations
        // et si possible qui partagent les mêmes tags
        // et si possible qui sont dans la même ville
        var usersWithSameFavoriteFoods = context.FavoriteFood
            .Where(f => f.UserId != userId && favoritesFoodUserConnected.Contains(f.FoodId))
            .Select(f => f.UserId)
            .Distinct()
            .ToList();

        var userWithSameTags = context.UserTag
            .Where(ut => tagsUserConnected.Contains(ut.UserId))
            .Select(ut => ut.UserId)
            .ToList();

        var restaurants = context.UserInvitation
            .Where(ui => ui.IsAccepted && (usersWithSameFavoriteFoods.Contains(ui.UserId) || userWithSameTags.Contains(ui.UserId)))
            .Join(context.Invitation, ui => ui.InvitationId, i => i.Id, (ui, i) => new { ui.UserId, i.Restaurant })
            .Where(Restaurant => Restaurant.Restaurant.UserId == userId)
            .Select(Restaurant => new RestaurantDto{
                Id = Restaurant.Restaurant.Id,
                Name = Restaurant.Restaurant.Name,
                Url = Restaurant.Restaurant.Url,
                Picture = Restaurant.Restaurant.Picture,
                Price = Restaurant.Restaurant.Price,
                VegetarianDish = Restaurant.Restaurant.VegetarianDish
            })
            .Distinct()
            .ToList();

        
        // Si user a déjà accepté ou envoyé des invitations (


        return new OkObjectResult(restaurants);
    }


    public async Task<ActionResult<Restaurant>> AddRestaurant(RestaurantDto request, int userId, [FromServices] DataContext context)
    {

        if (userId != null)
        {
            try
            {
                var user = context.Utilisateur.FirstOrDefault(u => u.Id == userId);
                var newLoc = new Localisation();

                if (user != null)
                {
                    var restaurantExist = await context.Restaurant.FirstOrDefaultAsync(r => r.Name == request.Name);
                    var localisationExist = await context.Localisation.FirstOrDefaultAsync(l => l.Lng == request.Lng && l.Lat == request.Lat);

                    if (restaurantExist != null)
                    {
                        return new BadRequestObjectResult("This restaurant already exists.");
                    }

                    //Check if localisation exist
                    if (localisationExist != null)
                    {
                        newLoc = localisationExist;
                    }
                    else
                    {
                        newLoc = new Localisation
                        {
                            Lat = request.Lat,
                            Lng = request.Lng,
                            Adress = request.Adress,
                            City = request.City,
                            ZipCode = request.ZipCode
                        };
                    }

                    var newRestaurant = new Restaurant
                    {
                        Name = request.Name,
                        Picture = request.Picture,
                        Price = (double)request.Price,
                        UserId = userId,
                        User = user,
                        Localisation = newLoc
                    };

                    context.Restaurant.Add(newRestaurant);
                    await context.SaveChangesAsync();

                    return new OkObjectResult($"The new restaurant {newRestaurant.Name} has been added");
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

    public async Task<IActionResult> Update(UpdateRestaurantDto request, int userId, int restaurantId, [FromServices] DataContext context)
    {
        if (userId != null)
        {
            try
            {
                var user = context.Utilisateur.FirstOrDefault(u => u.Id == userId);
                var restaurant = context.Restaurant.FirstOrDefault(r => r.Id == restaurantId && r.UserId == userId);

                if (user != null)
                {
                    if (restaurant != null)
                    {
                        if (request.Name != null)
                            restaurant.Name = request.Name;

                        if (request.Picture != null)
                            restaurant.Picture = request.Picture;

                        if (request.Url != null)
                            restaurant.Url = request.Url;

                        if (request.Price != null)
                            restaurant.Price = (double)request.Price;

                        if (request.VegetarianDish)
                            restaurant.VegetarianDish = request.VegetarianDish;

                        restaurant.UpdatedAt = DateTime.UtcNow;

                        await context.SaveChangesAsync();
                        return new OkObjectResult("Restaurant information updated successfully");
                    }
                    else
                    {
                        return new UnauthorizedObjectResult($"You are not authorized to edit this restaurant");
                    }
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
}

