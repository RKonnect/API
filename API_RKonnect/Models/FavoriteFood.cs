using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class FavoriteFood
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int FoodId { get; set; }
        public Food? Food { get; set; }
    }
}
