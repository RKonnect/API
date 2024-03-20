namespace API_RKonnect;

using API_RKonnect.Models;
using Microsoft.EntityFrameworkCore;
using API_RKonnect;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
    }

    public DbSet<User> Utilisateur { get; set; }
    public DbSet<Avatar> Avatar { get; set; }
    public DbSet<Restaurant> Restaurant { get; set; }
    public DbSet<Localisation> Localisation { get; set; }
    public DbSet<Invitation> Invitation { get; set; }
    public DbSet<UserAllergy> UserAllergy { get; set; }
    public DbSet<FavoriteFood> FavoriteFood { get; set; }
    public DbSet<UserTag> UserTag { get; set; }
    public DbSet<UserInvitation> UserInvitation { get; set; }
    public DbSet<Food> Food { get; set; }
    public DbSet<Tag> Tag { get; set; }


    // Ajouter par défaut 5 avatars
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Avatar>().HasData(
                       new Avatar { Id = 1, Name = "avatar1.png" },
                                  new Avatar { Id = 2, Name = "avatar2.png" },
                                             new Avatar { Id = 3, Name = "avatar3.png" },
                                                        new Avatar { Id = 4, Name = "avatar4.png" },
                                                                   new Avatar { Id = 5, Name = "avatar5.png" }
                                                                          );
    }
}