using API_RKonnect.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_RKonnect.Models
{
    public class User
    {
        private static Random random = new Random();

        private string GenerateUniqueNumber()
        {
            return random.Next(1000, 9999).ToString();
        }

        public User()
        {
            Pseudo += GenerateUniqueNumber();
        }

        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Pseudo { get; set; } = "Utilisateur#";
        public string? Email { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        public string? Biography { get; set; }
        public string? Avatar { get; set; }
        public UserGender? Gender { get; set; }
        public UserRole? Role { get; set; }
        public ICollection<Restaurant>? Restaurants { get; set; }
        public ICollection<UserTag>? UserTag { get; set; }
        public ICollection<UserAllergy>? Allergy { get; set; }
        public ICollection<FavoriteFood>? FavoriteFood { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
