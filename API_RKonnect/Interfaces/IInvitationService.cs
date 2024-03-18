using API_RKonnect;
using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Mvc;

public interface IInvitationService
{
    IActionResult GetInvitationById(int userId, [FromServices] DataContext context);
    Task<IActionResult> JoinInvitation(int invitationId, int userId, [FromServices] DataContext context);
    Task<ActionResult<Invitation>> AddInvitation(InvitationDto request, int userId, [FromServices] DataContext context);
}
