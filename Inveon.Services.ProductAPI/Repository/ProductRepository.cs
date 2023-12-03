using AutoMapper;
using Inveon.Services.ProductAPI.DbContexts;
using Inveon.Services.ProductAPI.Dto;
using Inveon.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using Inveon.Services.ProductAPI.Exceptions;

namespace Inveon.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        //Constructor Injection 
        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<ProductDto, Product>(productDto);
            //gelen ProductDto nun içindeki ProductId 0 dan büyük ise güncelleme yapılacak
            if (product.ProductId > 0)
            {
                _db.Products.Update(product);
            }
            else
            {
                //0 dan büyük değilse yeni bir kayıt eklenecek

                _db.Products.Add(product);
            }

            await _db.SaveChangesAsync();
            //kayıt eklendikten sonra databaseden eklenen product objesi geriye produtcDto olarak döndürülür
            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _db.Products.FirstOrDefaultAsync(u => u.ProductId == productId);
                if (product == null)
                {
                    return false;
                }

                _db.Products.Remove(product); //delete from Product where Id=productId
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            //linq select * from Product where Id=productId
            //{Id:1,Name : Product1}
            var product = await _db.Products
                .Select(Helpers.MapProductToDto)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                throw new NotFoundException(HttpStatusCode.NotFound, $"Product with ID {productId} not found");
            }
            
            return product;

        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            return await _db.Products
                .Select(Helpers.MapProductToDto)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByLabel(string labelName)
        {
            // this will get products based on label, it will be useful to filter based on label.
            return await _db.Products
                .Where(p => p.Labels.Any(l => l.Name == labelName))
                .Select(Helpers.MapProductToDto)
                .ToListAsync();
        }
    }
}

internal static class Helpers
{

    public static readonly Expression<Func<Product, ProductDto>> MapProductToDto = p => new ProductDto
    {
        ProductId = p.ProductId,
        Name = p.Name,
        CategoryName = p.CategoryName,
        ImageUrl = p.ImageUrl,
        Description = p.Description,
        HoverImageUrl = p.HoverImageUrl,
        Colours = p.Colours.Select(c => new ColourDto
        {
            Id = c.Id,
            Name = c.Name,
            Img = c.Img,
            Quantity = c.Quantity
        }).ToList(),
        Labels = p.Labels.Select(l => new LabelDto
        {
            Id = l.LabelId,
            Name = l.Name
        }).ToList(),
        Price = p.Price,
        Rating = new RatingDto
        {
            Count = p.Rating.Count,
            Rate = p.Rating.Rate,
        }
    };
}
