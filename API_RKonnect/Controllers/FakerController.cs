using API_RKonnect.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Faker;
using Enum = System.Enum;
using System.Security.Cryptography;

namespace API_RKonnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakerController : ControllerBase
    {

        [HttpPost("addFakeDatas")]
        public async Task<IActionResult> AddFakeDatas([FromServices] DataContext context)
        {
            var indexUsed = new List<int>();

            var fakeRestau = new List<(double Lat, double Lng, string Adress, string City, int ZipCode, string restauName, string Url, string Picture, double Price, bool VegetarianDish)>
            {
                (49.4441883, 1.0934149, "33 All. Eugène Delacroix", "Rouen", 76000, "L'ardoise", "", "https://localhost:7178/uploads/restau_ardoise.jpg", 25, true),
                (49.443127, 1.077645, "8 All. Eugène Delacroix", "Rouen", 76000, "EL PALAZZO", "", "https://localhost:7178/uploads/el_palazzo.jpg", 20, true),
                (49.4400381, 1.0977943, "260 Rue Martainville", "Rouen", 76000, "La Walsheim", "", "https://localhost:7178/uploads/la_walsheim.jpg", 20, false),
                (49.4414083, 1.0991352, "243 Rue Eau de Robec", "Rouen", 76000, "Ho Lamian", "", "https://localhost:7178/uploads/ho_lamian.jpg", 22, true),
                (49.4432727, 1.0863193, "15 Rue Thomas Corneille", "Rouen", 76000, "Rest'O'Rock", "", "https://localhost:7178/uploads/Rest_O_Rock.jpg", 23, true),
                (49.4439563, 1.0872589, "27 Rue Cauchoise", "Rouen", 76000, "Le Bistrot d'Arthur", "", "https://localhost:7178/uploads/bistrot_arthur.jpg", 18, true),
                (49.439598, 1.0998013, "144 Rue Martainville", "Rouen", 76000, "Romy", "", "https://localhost:7178/uploads/romy.jpg", 18, true),
            };

            var listFood = new List<(string Name, string Icon)>
            {
                ("Oeuf", "fa-solid fa-egg"),
                ("Poisson", "fa-solid fa-fish"),
                ("Pomme", "fa-solid fa-apple-whole"),
                ("Piment", "fa-solid fa-pepper-hot"),
                ("Céréale", "fa-solid fa-wheat-awn"),
                ("Fromage", "fa-solid fa-cheese"),
                ("Viande", "fa-solid fa-drumstick-bite")
            };

            var listTag = new List<string>
            {
                "Cinema", "Sport", "Lecture", "Jeux-vidéo", "Musique", "Crypto", "Peinture", "Art",
                "Photographie", "Voyage", "Cuisine", "Technologie", "Jardinage","Mode", "Fitness", 
                "Écriture", "Théâtre", "Danse", "Science", "Astronomie"
            };

            for (int i = 0; i < 18; i++)
            {
                var random = new Random();
                var genders = Enum.GetValues(typeof(Enums.UserGender));
                var roles = Enum.GetValues(typeof(Enums.UserRole));

                var FakerName = Name.Last();
                var FakerSurname = Name.Last();
                var FakerPassword = "azerty";
                var randomGender = (Enums.UserGender)genders.GetValue(random.Next(genders.Length));
                var randomRole = (Enums.UserRole)roles.GetValue(random.Next(roles.Length));
                CreatePasswordHash(FakerPassword, out byte[] passwordHash, out byte[] passwordSalt);

                var newUser = new User
                {
                    Name = FakerName,
                    Surname = FakerSurname,
                    Email = $"{FakerName}.{FakerSurname}@gmail.com",
                    Pseudo = $"{FakerName}{FakerSurname}",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Biography = Lorem.Paragraph(),
                    //Avatar =
                    Gender = randomGender,
                    Role = randomRole,
                    DateOfBirth = Identification.DateOfBirth()
                };
                context.Utilisateur.Add(newUser);
                Console.WriteLine($"Nouvel utilisateur créé : {newUser.Name} {newUser.Surname}");

                //ADD RESTAURANT
                if (newUser.Role == Enums.UserRole.Professional)
                {
                    int index;
                    do
                    {
                        index = random.Next(fakeRestau.Count);
                    } 
                    while (indexUsed.Contains(index));

                    indexUsed.Add(index);
                    var restauInfo = fakeRestau[index];

                    var newLoc = new Localisation
                    {
                        Lat = restauInfo.Lat,
                        Lng = restauInfo.Lng,
                        Adress = restauInfo.Adress,
                        City = restauInfo.City,
                        ZipCode = restauInfo.ZipCode,
                    };

                    var newRestaurant = new Restaurant
                    {
                        Name = restauInfo.restauName,
                        Picture = restauInfo.Picture,
                        Price = (double)restauInfo.Price,
                        UserId = newUser.Id,
                        User = newUser,
                        Localisation = newLoc
                    };
                    context.Restaurant.Add(newRestaurant);
                    Console.WriteLine($"Nouveau restaurant créé : {newRestaurant.Name}");

                }

                await context.SaveChangesAsync();
            }

            return Ok("Les fakes datas ont bien été ajoutées !");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
