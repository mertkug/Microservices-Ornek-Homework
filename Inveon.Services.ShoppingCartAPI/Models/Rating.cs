
namespace Inveon.Services.ShoppingCartAPI.Models;

public class Rating
{
    public int Id { get; set; }
    public double Rate { get; set; }
    public int Count { get; set; }
    
    public int ProductId { get; set; }
    public Product? Product { get; set; }
}