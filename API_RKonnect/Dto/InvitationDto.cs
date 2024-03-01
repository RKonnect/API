using API_RKonnect.Enums;

namespace API_RKonnect.Dto
{
    public class InvitationDto
    {
        public int RestaurantId { get; set; }
        public int UserMax { get; set; }
        public PaymentType? PaymentType { get; set; }
        public DateTime InvitationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
