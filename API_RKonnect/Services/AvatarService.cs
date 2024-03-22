using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace API_RKonnect.Services
{
    public class AvatarService : IAvatarService
    {
        public IActionResult GetAll([FromServices] DataContext context)
        {
            var avatars = context.Avatar
                .AsNoTracking()
                .ToList();

            return new OkObjectResult(avatars);
        }
    }
}
