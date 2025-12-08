using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.Interface.IRepository
{
    public interface IUserAddressRepository
    {
        Task<List<DeliveryAddress>> GetAddressesByUserIdAsync(int userId);
    }
}