using System.ComponentModel.DataAnnotations;

namespace API_RKonnect.Models
{
    public class Food
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        public static Food AddFood(string name, DataContext context)
        {
            Food newFood = new Food
            {
                Name = name
            };

            context.Food.Add(newFood);
            context.SaveChanges();
            return newFood;
        }
    }
}
