using RestaurantBookingSystem.DTO;

namespace RestaurantBookingSystem.Interface.IService
{
    public interface IUserAddressService
    {
        Task<List<UserAddressDto>> GetAddressesByUserIdAsync(int userId);
    }
}