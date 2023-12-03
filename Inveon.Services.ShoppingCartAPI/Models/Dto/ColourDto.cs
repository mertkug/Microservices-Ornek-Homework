using System.ComponentModel.DataAnnotations;

namespace Inveon.Services.ShoppingCartAPI.Models.Dto;

public class ColourDto
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Img { get; set; }
    public int Quantity { get; set; }
    
    public int ProductId { get; set; }
    public ProductDto Product { get; set; }
}