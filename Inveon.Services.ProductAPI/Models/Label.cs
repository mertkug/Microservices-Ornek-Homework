using Inveon.Services.ProductAPI.Dto;

namespace Inveon.Services.ProductAPI.Models;

public class Label
{
    public int LabelId { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}