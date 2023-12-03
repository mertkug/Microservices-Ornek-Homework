namespace Inveon.Services.ShoppingCartAPI.Models.Dto;

public class RatingDto
{
    public int Id { get; set; }
    public double Rate { get; set; }
    public int Count { get; set; }
    
    public int ProductId { get; set; }
    public ProductDto? Product { get; set; }
}