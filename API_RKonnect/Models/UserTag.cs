using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class UserTag
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
