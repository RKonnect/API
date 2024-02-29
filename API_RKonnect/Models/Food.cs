using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class Food
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
