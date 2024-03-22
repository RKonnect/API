using API_RKonnect.Models;

namespace API_RKonnect.Dto
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public double Price { get; set; }
        public bool VegetarianDish { get; set; } = false;
        public double Lat {  get; set; }
        public double Lng { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public Localisation? Localisation { get; set; }
        public PublicUserDto? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
