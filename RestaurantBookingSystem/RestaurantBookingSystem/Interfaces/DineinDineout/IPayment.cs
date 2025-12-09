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
    }
}
