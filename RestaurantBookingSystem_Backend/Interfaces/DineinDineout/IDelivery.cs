using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.Interface
{
    public interface IDelivery
    {
        Task<Delivery> CreateDeliveryAsync(int orderId, int? addressId);
        Task<Delivery?> GetDeliveryByIdAsync(int deliveryId);
        Task<IEnumerable<Delivery>> GetDeliveriesByUserIdAsync(int userId);
        Task<Delivery?> UpdateDeliveryStatusAsync(int deliveryId, DeliveryStatus status);
    }
}
