using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inveon.Services.FavouriteProduct.Models;

public class ProductDto
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ProductId { get; set; }
    public string ImageUrl { get; set; }
    
    [Required]
    public string Name { get; set; }
    [Range(1, 1000)]
    public decimal Price { get; set; }
    
    public string CategoryName { get; set; }
    public string Description { get; set; }

}