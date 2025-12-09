using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.Interface
{
    public interface IDeliveryPerson
    {
        Task<IEnumerable<DeliveryPerson>> GetAllAsync();
        Task<DeliveryPerson?> GetByIdAsync(int id);
        Task<DeliveryPerson> CreateAsync(DeliveryPerson deliveryPerson);
        Task<DeliveryPerson?> UpdateAsync(DeliveryPerson deliveryPerson);
        Task<bool> UpdateStatusAsync(int id, DeliveryPersonStatus status);
        Task<int> UpdateOtpAsync(int id, int otp);
        Task UpdateDeliveryStatsAsync(int deliveryPersonId);
        Task<(int totalDeliveries, double averageRating)> CalculateStatsAsync(int deliveryPersonId);
    }
}
