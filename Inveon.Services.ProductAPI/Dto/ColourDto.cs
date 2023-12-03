using System.ComponentModel.DataAnnotations;
using Inveon.Services.ProductAPI.Dto;

namespace Inveon.Services.ProductAPI.Models;

public class ColourDto
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Img { get; set; }
    public int Quantity { get; set; }
    public ProductDto Product { get; set; }
}