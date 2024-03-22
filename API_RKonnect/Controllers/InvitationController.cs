using API_RKonnect.Dto;
using API_RKonnect.Enums;
using API_RKonnect.Models;
using API_RKonnect.Services;
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

        [Authorize]
        [HttpGet("getById/{userId}")]
        public IActionResult GetInvitationById(int userId, [FromServices] DataContext context, [FromServices] IInvitationService invitationService)
        {
            return invitationService.GetInvitationById(userId, context);
        }

        [Authorize]
        [HttpGet("join/{invitationId}")]
        public async Task<IActionResult> JoinInvitation(int invitationId, [FromServices] DataContext context, [FromServices] IInvitationService invitationService)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user);

            return await invitationService.JoinInvitation(invitationId, userId, context);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Invitation>> AddInvitation(InvitationDto request, [FromServices] DataContext context, [FromServices] IInvitationService invitationService)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user);

            return await invitationService.AddInvitation(request, userId, context);
        }

    }
}
