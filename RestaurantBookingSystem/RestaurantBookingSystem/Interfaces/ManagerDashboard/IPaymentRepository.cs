using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<Payment>> GetByRestaurantAndDateRangeAsync(int restaurantId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetByRestaurantAndMethodAsync(int restaurantId, PayMethod method);
        Task<IEnumerable<Payment>> GetTodayPaymentsAsync(int restaurantId);
        Task<object> GetPaymentSummaryAsync(int restaurantId, DateTime? startDate, DateTime? endDate);
    }
}