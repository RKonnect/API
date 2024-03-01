using API_RKonnect.Models;

namespace API_RKonnect.Dto
{
    public class GetInvitationInfoDto
    {
        public int InvitationId { get; set; }
        public bool IsAccepted { get; set; }
        public List<PublicUserDto> Guests { get; set; }
    }
}
