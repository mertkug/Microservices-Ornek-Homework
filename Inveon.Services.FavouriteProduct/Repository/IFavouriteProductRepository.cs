using Inveon.Services.FavouriteProduct.Models;

namespace Inveon.Services.FavouriteProduct.Repository;

public interface IFavouriteProductRepository
{
    Task<List<FavouriteDto>> GetFavouritesById(string userId);
    Task AddToFavourites(string userId, ProductDto product);
}