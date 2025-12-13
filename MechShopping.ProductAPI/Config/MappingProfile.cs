using AutoMapper;
using MechShopping.ProductAPI.Model;
using MechShopping.ProductAPI.Data.ValueObjects;

namespace MechShopping.ProductAPI.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductVO>().ReverseMap();
        }
    }
}
