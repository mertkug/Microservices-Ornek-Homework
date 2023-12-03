using System.ComponentModel.DataAnnotations;

namespace Inveon.Services.ProductAPI.Models;

public class Colour
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Img { get; set; }
    public int Quantity { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }

}