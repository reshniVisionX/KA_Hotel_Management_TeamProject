using AutoMapper;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<CartRequestDto, Cart>()
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Cart, CartResponseDto>()
                .ForMember(dest => dest.ItemName, opt => opt.Ignore())
                .ForMember(dest => dest.RestaurantName, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.IsVegetarian, opt => opt.Ignore());
        }
    }
}