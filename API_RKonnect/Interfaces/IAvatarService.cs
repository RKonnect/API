using API_RKonnect;
using Microsoft.AspNetCore.Mvc;

public interface IAvatarService
{
    IActionResult GetAll([FromServices] DataContext context);
}

