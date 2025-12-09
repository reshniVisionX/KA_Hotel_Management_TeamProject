using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.Interfaces.Admin
{
    public interface IAdminDeliveryService
    {
      

        Task<IEnumerable<DeliveryAddress>> GetUserAddresses(int userId);
        Task<DeliveryAddress> AddAddress(DeliveryAddress address);
        Task<bool> ChangeDefaultAddress(int userId, int deliveryAddressId);
    }
}
