using API_RKonnect.Enums;

namespace API_RKonnect.Dto
{
    public class UserDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Pseudo {  get; set; }
        public string? Biography { get; set; }
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public UserRole? Role { get; set; }
    }
}
