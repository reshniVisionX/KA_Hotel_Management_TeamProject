using AutoMapper;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Review, ReviewResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}".Trim() : ""))
                .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.RestaurantName : ""));

            CreateMap<DineIn, TableResponseDto>()
                .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurants != null ? src.Restaurants.RestaurantName : "Unknown Restaurant"))
                .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}