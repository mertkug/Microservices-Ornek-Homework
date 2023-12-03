using Inveon.Services.FavouriteProduct.Models;
using Microsoft.EntityFrameworkCore;

namespace Inveon.Services.FavouriteProduct.DatabaseContext;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Favourite> FavoriteProducts { get; set; }
}