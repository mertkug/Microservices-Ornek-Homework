using Inveon.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Inveon.Services.ShoppingCartAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Label> Labels { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the specific table (e.g., Products) to use a different name
            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .Property(p => p.ProductId)
                .ValueGeneratedNever(); // Indicates that the database should not generate values for ProductId

            modelBuilder.Entity<Colour>()
                .ToTable("Colours")
                .Property(p => p.Id)
                .ValueGeneratedNever();
            
            modelBuilder.Entity<Rating>()
                .ToTable("Ratings")
                .Property(p => p.Id)
                .ValueGeneratedNever();
            
            modelBuilder.Entity<Label>()
                .Property(p => p.LabelId)
                .ValueGeneratedNever();
            
            modelBuilder.Entity<Product>()
                .HasOne(c => c.Rating)
                .WithOne(e => e.Product)
                .HasForeignKey<Rating>(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Colour>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Colours)
                .HasForeignKey(c => c.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Labels)
                .WithMany(e => e.Products);
        }

    }
}
