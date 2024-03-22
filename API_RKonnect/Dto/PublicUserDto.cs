namespace API_RKonnect.Dto
{
    public class PublicUserDto
    {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set;}
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
