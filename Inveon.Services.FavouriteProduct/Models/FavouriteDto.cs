using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inveon.Services.FavouriteProduct.Models;

public class FavouriteDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual ProductDto Product { get; set; }
}