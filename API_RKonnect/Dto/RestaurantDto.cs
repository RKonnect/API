using API_RKonnect.Models;

namespace API_RKonnect.Dto
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public double? Price { get; set; }
        public bool VegetarianDish { get; set; } = false;
        public PublicUserDto User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
