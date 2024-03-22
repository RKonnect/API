using API_RKonnect.Dto;
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
using API_RKonnect.Services;
using System.Data.Entity;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [Authorize]
        [HttpGet("getAll")]
        public IActionResult GetAll([FromServices] DataContext context, IUserService userService)
        {
            return userService.GetAll(context);
        }

        [Authorize]
        [HttpGet("getById/{userId}")]
        public IActionResult GetById(int userId, [FromServices] DataContext context, IUserService userService)
        {
            return userService.GetById(userId, context);
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto request, [FromServices] DataContext context, IUserService userService)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user);

            return await userService.UpdateUser(request, userId, context);
        }

        [Authorize]
        [HttpPost("changeAvatar/{avatarId}")]
        public async Task<IActionResult> ChangeAvatar(int avatarId, [FromServices] DataContext context, IUserService userService)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user);

            return await userService.ChangeAvatar(userId, avatarId, context);
        }

        [Authorize]
        [HttpGet("addAllergy/{allergyId}")]
        public async Task<IActionResult> addAllergy(int allergyId, [FromServices] DataContext context, IUserService userService)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user);

            return await userService.addAllergy(allergyId, userId, context);
        }

        [Authorize]
        [HttpGet("addFavorite/{favoriteId}")]
        public async Task<IActionResult> addFavorite(int favoriteId, [FromServices] DataContext context, IUserService userService)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user);

            return await userService.addFavorite(favoriteId, userId, context);
        }

        [Authorize]
        [HttpGet("addTag/{tagId}")]
        public async Task<IActionResult> addTag(int tagId, [FromServices] DataContext context, IUserService userService)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user);

            return await userService.addTag(tagId, userId, context);
        }

    }
}
