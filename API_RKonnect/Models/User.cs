using API_RKonnect.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Pseudo { get; set; }
        public string? Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? Biography { get; set; }
        public string? Avatar { get; set; }
        public UserGender? Gender { get; set; }
        public UserRole? Role { get; set; }
        public ICollection<UserAllergy>? Allergy { get; set; }
        public ICollection<FavoriteFood>? FavoriteFood { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
