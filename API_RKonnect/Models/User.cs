using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Pseudo { get; set; }
        public string? Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? Biography { get; set; }
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public string? Role { get; set; }
        public ICollection<UserAllergy>? Allergy { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public void AjouterAllergy(int userId, int foodId, DataContext context)
        {
            if (userId <= 0 || foodId <= 0)
            {
                throw new ArgumentNullException("Au moins un id est de valeur 0 ou négatif");
            }

            User utilisateur = context.Utilisateur.FirstOrDefault(u => u.Id == userId);
            Food food = context.Food.FirstOrDefault(f => f.Id == foodId);

            if (utilisateur != null && food != null)
            {
                Allergy.Add(new UserAllergy
                {
                    UserId = userId,
                    User = utilisateur,
                    FoodId = foodId,
                    Food = food,
                });

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Utilisateur ou aliment non trouvé.");
            }
        }
    }
}
