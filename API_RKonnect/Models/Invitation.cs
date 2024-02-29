using API_RKonnect.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Restaurant Restaurant { get; set; }
        [Required]
        public int UserMax { get; set; }
        public PaymentType? PaymentType { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime InvitationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
