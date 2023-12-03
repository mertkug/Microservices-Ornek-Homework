using System.Linq.Expressions;
using Inveon.Services.FavouriteProduct.DatabaseContext;
using Inveon.Services.FavouriteProduct.Exceptions;
using Inveon.Services.FavouriteProduct.Models;
using Microsoft.EntityFrameworkCore;

namespace Inveon.Services.FavouriteProduct.Repository;

public class FavouriteProductRepository : IFavouriteProductRepository
{
    private readonly ApplicationDbContext _db;
    
    public FavouriteProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<List<FavouriteDto>> GetFavouritesById(string userId)
    {
        return _db.FavoriteProducts
            .Where(fp => fp.UserId == userId)
            .Include(fp => fp.Product)
            .Select(Helpers.MapFavProductToDto)
            .ToListAsync();
    }

    public async Task AddToFavourites(string userId, ProductDto product)
    {
        var productDb = _db.Products.FirstOrDefault(x => x.ProductId == product.ProductId);

        var mappedProduct = new Product
        {
            Name = product.Name,
            CategoryName = product.CategoryName,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Price = product.Price
        };
        if (productDb == null)
        {
            _db.Products.Add(mappedProduct);
        }

        var fav = new Favourite
        {
            ProductId = product.ProductId,
            UserId = userId,
            Product = mappedProduct
        };
        var favoriteProductFromDb = _db.FavoriteProducts
            .FirstOrDefault(fp => fp.UserId == userId && fp.ProductId == product.ProductId);

        if (favoriteProductFromDb != null)
        {
            throw new AlreadyExitsException("It's already in favourites");
        }
        
        _db.FavoriteProducts.Add(fav);
        try
        {
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}
internal static class Helpers
{

    public static readonly Expression<Func<Favourite, FavouriteDto>> MapFavProductToDto = f => new FavouriteDto
    {
        Id = f.ProductId,
        Product = new ProductDto
        {
            CategoryName = f.Product.CategoryName,
            Description = f.Product.Description,
            ImageUrl = f.Product.ImageUrl,
            Name = f.Product.Name,
            Price = f.Product.Price
        }
    };
}