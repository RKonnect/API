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
    public DbSet<UserAllergy> UserAllergy { get; set; }
    public DbSet<FavoriteFood> FavoriteFood { get; set; }
    public DbSet<UserTag> UserTag { get; set; }
    public DbSet<Food> Food { get; set; }
    public DbSet<Tag> Tag { get; set; }
}