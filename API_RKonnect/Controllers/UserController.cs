﻿using API_RKonnect.Dto;
using API_RKonnect.Enums;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var users = _context.Utilisateur
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
                    Tags = user.UserTag.Select(ut => new TagDto { Title = ut.Tag.Title, Icon = ut.Tag.Icon }).ToList(),
                    Allergy = user.Allergy.Select(ua => new FoodDto { Name = ua.Food.Name }).ToList(),
                    FavoriteFood = user.FavoriteFood.Select(ua => new FoodDto { Name = ua.Food.Name }).ToList(),
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                })
                .ToList();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("getById/{userId}")]
        public IActionResult GetById(int userId)
        {
            var selectedUser = _context.Utilisateur
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
                    Tags = user.UserTag.Select(ut => new TagDto { Title = ut.Tag.Title, Icon = ut.Tag.Icon }).ToList(),
                    Allergy = user.Allergy.Select(ua => new FoodDto { Name = ua.Food.Name }).ToList(),
                    FavoriteFood = user.FavoriteFood.Select(ua => new FoodDto { Name = ua.Food.Name }).ToList(),
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                })
                .SingleOrDefault();

            if (selectedUser == null)
            {
                return NotFound();
            }

            return Ok(selectedUser);
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                try
                {
                    int userIdInt = int.Parse(userId);
                    var user = _context.Utilisateur.FirstOrDefault(u => u.Id == userIdInt);

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

                        await _context.SaveChangesAsync();
                        return Ok($"User information updated successfully");
                    }
                    else
                    {
                        return NotFound("User not found");
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

        [Authorize]
        [HttpGet("addAllergy/{allergyId}")]
        public async Task<IActionResult> addAllergy(int allergyId)
        {
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

        [Authorize]
        [HttpGet("addFavorite/{favoriteId}")]
        public async Task<IActionResult> addFavorite(int favoriteId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var food = _context.Food.FirstOrDefault(f => f.Id == favoriteId);

            if (userId != null && favoriteId != 0)
            {
                try
                {
                    int userIdInt = int.Parse(userId);
                    var user = _context.Utilisateur.FirstOrDefault(u => u.Id == userIdInt);

                    if (user != null && food != null)
                    {

                        user.FavoriteFood ??= new List<FavoriteFood>();

                        // Check if the user already has the favorite food
                        if (!user.FavoriteFood.Any(a => a.FoodId == favoriteId))
                        {
                            user.FavoriteFood.Add(new FavoriteFood
                            {
                                UserId = userIdInt,
                                User = user,
                                FoodId = favoriteId,
                                Food = food,
                            });

                            await _context.SaveChangesAsync();
                            return Ok($"Food with ID {favoriteId} added for user with ID {userId}");
                        }
                        else
                        {
                            return Conflict("User already has this favorite food");
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

        [Authorize]
        [HttpGet("addTag/{tagId}")]
        public async Task<IActionResult> addTag(int tagId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tag = _context.Tag.FirstOrDefault(t => t.Id == tagId);

            if (userId != null && tagId != 0)
            {
                try
                {
                    int userIdInt = int.Parse(userId);
                    var user = _context.Utilisateur.FirstOrDefault(u => u.Id == userIdInt);

                    if (user != null && tag != null)
                    {

                        user.UserTag ??= new List<UserTag>();

                        // Check if the user already has the favorite food
                        if (!user.UserTag.Any(t => t.TagId == tagId))
                        {
                            user.UserTag.Add(new UserTag
                            {
                                UserId = userIdInt,
                                User = user,
                                TagId = tagId,
                                Tag = tag,
                            });

                            await _context.SaveChangesAsync();
                            return Ok($"Tag with ID {tagId} added for user with ID {userId}");
                        }
                        else
                        {
                            return Conflict("User already has this tag");
                        }
                    }
                    else
                    {
                        return NotFound("User or Tag not found");
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
