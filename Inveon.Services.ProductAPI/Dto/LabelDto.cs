using Inveon.Services.ProductAPI.Models;

namespace Inveon.Services.ProductAPI.Dto;

public class LabelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ProductDto> Products { get; set; }
}