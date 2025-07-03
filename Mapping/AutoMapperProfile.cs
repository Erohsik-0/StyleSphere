using AutoMapper;
using StyleSphere.Domain.Entities;
using StyleSphere.Models.ViewModel;
using StyleSphere.ViewModels;

namespace StyleSphere.Mapping
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();

            CreateMap<CartItem, CartViewModel>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.name))
            .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.Product!.image))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product!.price));

        }

    }
}
