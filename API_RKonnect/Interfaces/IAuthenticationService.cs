using System.Threading.Tasks;
using API_RKonnect;
using API_RKonnect.Dto;
using API_RKonnect.Models;
using Microsoft.AspNetCore.Mvc;

public interface IAuthenticationService
{
    Task<ActionResult<User>> Register(AuthDto request, [FromServices] DataContext context);

    Task<ActionResult<string>> Login(AuthDto request, [FromServices] DataContext context);
}
