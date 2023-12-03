
using Inveon.Services.ProductAPI.Models;

namespace Inveon.Services.ProductAPI.Dto;

public class RatingDto
{
    public int Id { get; set; }
    public double Rate { get; set; }
    public int Count { get; set; }
    
    public ProductDto? Product { get; set; }
}