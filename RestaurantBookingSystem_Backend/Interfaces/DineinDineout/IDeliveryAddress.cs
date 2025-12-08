using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.Interface
{
    public interface IDeliveryAddress
    {
        Task<IEnumerable<DeliveryAddress>> GetAddressesByUserAsync(int userId);
        Task<DeliveryAddress?> GetAddressByIdAsync(int addressId);
        Task<DeliveryAddress?> GetDefaultAddressAsync(int userId);
        Task<DeliveryAddress> AddAddressAsync(DeliveryAddress address);
        Task<DeliveryAddress> UpdateAddressAsync(DeliveryAddress address);
        Task<bool> DeleteAddressAsync(int addressId);
        Task<bool> SetDefaultAddressAsync(int userId, int addressId);
        Task<IEnumerable<DeliveryAddress>> GetUserAddresses(int userId);
    }
}
