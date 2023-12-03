using AutoMapper;
using Inveon.Services.ProductAPI.Dto;
using Inveon.Services.ProductAPI.Models;

namespace Inveon.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto>();
                
                config.CreateMap<RatingDto, Rating>();
                config.CreateMap<Rating, RatingDto>();
                
                config.CreateMap<ColourDto, Colour>();
                config.CreateMap<Colour, ColourDto>();
                
                // config.CreateMap<LabelProductDto, LabelProduct>();
                // config.CreateMap<LabelProduct, LabelProductDto>();
            });

            return mappingConfig;
        }
    }
}
