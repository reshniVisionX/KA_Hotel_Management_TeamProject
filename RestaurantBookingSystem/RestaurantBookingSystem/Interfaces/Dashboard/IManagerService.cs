using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Model.Manager;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IManagerService
    {
        Task<(ManagerDetails Manager, Restaurants Restaurant)> RegisterManagerWithRestaurantAsync(ManagerRegisterDTO dto);
    }
}
