using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class UserInvitation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InvitationId { get; set; }
        [Required]
        public Invitation Invitation { get; set; } = new Invitation();
        [Required]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; } = new User();
    }
}
