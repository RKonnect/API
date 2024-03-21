using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class Avatar
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
