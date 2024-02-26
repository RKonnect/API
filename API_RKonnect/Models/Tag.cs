using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
    }
}
