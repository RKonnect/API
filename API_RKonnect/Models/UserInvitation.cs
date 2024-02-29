using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class UserInvitation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InvitationId { get; set; }
        public Invitation Invitation { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public bool IsAccepted { get; set; } = false;
    }
}
