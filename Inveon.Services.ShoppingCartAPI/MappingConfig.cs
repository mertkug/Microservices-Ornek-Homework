﻿using AutoMapper;
using Inveon.Services.ShoppingCartAPI.Models;
using Inveon.Services.ShoppingCartAPI.Models.Dto;

namespace Inveon.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
                config.CreateMap<Cart, CartDto>().ReverseMap();
                config.CreateMap<Label, LabelDto>().ReverseMap();
                config.CreateMap<Colour, ColourDto>().ReverseMap();
                config.CreateMap<Rating, RatingDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
