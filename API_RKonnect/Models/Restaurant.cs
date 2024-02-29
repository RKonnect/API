using System.ComponentModel.DataAnnotations;
namespace API_RKonnect.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        public string? Url { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public double Price { get; set; }
        public bool VegetarianDish {  get; set; } = false;
        public int UserId { get; set; }
        public User? User { get; set; }
        public Localisation Localisation { get; set; } = new Localisation();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
