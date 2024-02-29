using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Restaurant Restaurant { get; set; } = new Restaurant();
        [Required]
        public int UserMax { get; set; }
        [Required]
        public DateTime InvitationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
