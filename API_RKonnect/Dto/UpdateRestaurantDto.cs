namespace API_RKonnect.Dto
{
    public class UpdateRestaurantDto
    {
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public double? Price { get; set; }
        public bool VegetarianDish { get; set; } = false;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
