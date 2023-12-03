using Inveon.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Inveon.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Colour>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Colours)
                .HasForeignKey(c => c.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Labels)
                .WithMany(e => e.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "LabelProduct",
                    r => r.HasOne<Label>().WithMany().HasForeignKey("LabelId").OnDelete(DeleteBehavior.Cascade),
                    l => l.HasOne<Product>().WithMany().HasForeignKey("ProductId").OnDelete(DeleteBehavior.Cascade),
                    je =>
                    {
                        je.HasKey("ProductId", "LabelId");
                        je.HasData(new { LabelId = 1, ProductId = 1 });
                    }
                );


            modelBuilder.Entity<Product>()
                .HasOne(c => c.Rating)
                .WithOne(e => e.Product)
                .HasForeignKey<Rating>(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Green Dress For Woman",
                Price = 15,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "samosa.jpg",
                CategoryName = "Appetizer",
                HoverImageUrl = "hovered.jpg",
            });

            modelBuilder.Entity<Rating>().HasData(new Rating
            {
                Id = 1,
                Rate = 3.3,
                Count = 100,
                ProductId = 1
            });

            modelBuilder.Entity<Label>().HasData(new Label
            {
                LabelId = 1,
                Name = "Trending"
            });

            modelBuilder.Entity<Colour>().HasData(new Colour
            {
                Id = 1,
                Img = "greenimg",
                Name = "green",
                Quantity = 3,
                ProductId = 1
            });

            modelBuilder.Entity<Colour>().HasData(new Colour
            {
                Id = 2,
                Img = "redimg",
                Name = "red",
                Quantity = 2,
                ProductId = 1
            });
        }
    }
}
