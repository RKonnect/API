using API_RKonnect.Enums;
using API_RKonnect.Models;

namespace API_RKonnect.Dto
{
    public class GetUserInfoDto
    {
        public GetUserInfoDto()
        {
            Tags = new List<TagDto>();
            Allergy = new List<FoodDto>();
            FavoriteFood = new List<FoodDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Pseudo { get; set; }
        public string? Email { get; set; }
        public string? Biography { get; set; }
        public string? Avatar { get; set; }
        public UserGender? Gender { get; set; }
        public UserRole? Role { get; set; }
        public ICollection<TagDto> Tags { get; set; }
        public ICollection<FoodDto>? Allergy { get; set; }
        public ICollection<FoodDto>? FavoriteFood { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
