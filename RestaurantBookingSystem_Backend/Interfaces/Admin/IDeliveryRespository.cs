using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interfaces.Admin
{
    public interface IDeliveryRespository
    {
        Task<IEnumerable<DeliveryAddress>> GetUserAddressesAsync(int userId);
        Task<DeliveryAddress?> GetAddressByIdAsync(int addressId);
        Task<DeliveryAddress> AddAsync(DeliveryAddress address);
        Task SaveChangesAsync();

        Task<IEnumerable<MenuList>> GetAllMenuListAsync();

        Task<Delivery?> GetDeliveryWithRelationsAsync(int deliveryId);
        Task UpdateDeliveryAsync(Delivery delivery);
        Task UpdateDeliveryPersonAsync(DeliveryPerson person);
        Task UpdateOrderAsync(Orders order);
        Task<IEnumerable<Delivery>> GetDeliveriesByPersonAsync(int deliveryPersonId);
    }
}
