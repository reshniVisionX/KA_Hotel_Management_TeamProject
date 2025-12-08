using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.Interfaces.Admin
{
    public interface IDeliveryRespository
    {
        Task<IEnumerable<DeliveryAddress>> GetUserAddressesAsync(int userId);
        Task<DeliveryAddress?> GetAddressByIdAsync(int addressId);
        Task<DeliveryAddress> AddAsync(DeliveryAddress address);
        Task SaveChangesAsync();
    }
}
