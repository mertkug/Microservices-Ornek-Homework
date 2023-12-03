using Inveon.Services.FavouriteProduct.Models;
using Inveon.Services.FavouriteProduct.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Inveon.Services.FavouriteProduct.Controllers;

[ApiController]
[Route("api/favorites")]
public class FavouriteProductController : ControllerBase
{
    private readonly IFavouriteProductRepository _favouriteProductRepository;
    private ResponseDto _response;

    public FavouriteProductController(IFavouriteProductRepository favouriteProductRepository)
    {
        _favouriteProductRepository = favouriteProductRepository;
        _response = new ResponseDto();
    }

    [HttpGet("{userId}")]
    public async Task<object> GetFavouriteById(string userId)
    {
        try
        {
            var favourites = await _favouriteProductRepository.GetFavouritesById(userId);
            _response.Result = favourites;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }
    
    [HttpPost]
    public Task<object> AddToFavourites(FavouriteDto favoriteProductDto)
    {
        try
        {
            _favouriteProductRepository.AddToFavourites(favoriteProductDto.UserId, favoriteProductDto.Product);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return Task.FromResult<object>(_response);
    }

}