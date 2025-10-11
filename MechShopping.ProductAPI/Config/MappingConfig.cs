using AutoMapper;
using MechShopping.ProductAPI.Data.ValueObjects;
using MechShopping.ProductAPI.Model;

namespace MechShopping.ProductAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            { 
                config.CreateMap<ProductVO, Product>();
                config.CreateMap<Product, ProductVO>();
            });
            return mappingConfig;
        }
    }
}
