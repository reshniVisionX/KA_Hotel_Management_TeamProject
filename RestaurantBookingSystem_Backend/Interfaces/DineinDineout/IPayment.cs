using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IPayment
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId);
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> UpdateAsync(Payment payment);

        // --- Delivery --
        Task<DeliveryPerson?> GetFirstAvailableAsync();
        Task<DeliveryAddress?> GetDefaultAddressAsync(int userId);

        Task<Delivery> CreateAsync(Delivery delivery);
        Task UpdateAsync(DeliveryPerson person);



    }
}
