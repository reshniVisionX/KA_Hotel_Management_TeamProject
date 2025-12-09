using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<Orders>> GetByRestaurantAndDateRangeAsync(int restaurantId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Orders>> GetByRestaurantAndStatusAsync(int restaurantId, OrderStatus status);
        Task<IEnumerable<Orders>> GetTodayOrdersAsync(int restaurantId);
        Task<object> GetOrderSummaryAsync(int restaurantId, DateTime? startDate, DateTime? endDate);
    }
}