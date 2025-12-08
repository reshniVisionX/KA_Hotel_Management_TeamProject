using RestaurantBookingSystem.DTOs.Admin;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interfaces.Admin
{
    public interface IAdminDeliveryService
    {
      

        Task<IEnumerable<DeliveryAddress>> GetUserAddresses(int userId);
        Task<DeliveryAddress> AddAddress(DeliveryAddress address);
        Task<bool> ChangeDefaultAddress(int userId, int deliveryAddressId);
        Task<IEnumerable<MenuList>> GetAllMenuListAsync();
        Task<bool> CompleteDeliveryAsync(int deliveryId);

        Task<List<DeliveryPersonHistory>> GetDeliveriesForPersonAsync(int deliveryPersonId);

    }
}
