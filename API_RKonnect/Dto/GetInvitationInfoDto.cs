using API_RKonnect.Models;
using System.Collections.Generic;

namespace API_RKonnect.Dto
{
    public class GetInvitationInfoDto
    {
        public int InvitationId { get; set; }
        public List<PublicUserDto> Guests { get; set; }
        public PublicUserDto Host {  get; set; }
        public RestaurantDto Restaurants { get; set;}
    }
}
