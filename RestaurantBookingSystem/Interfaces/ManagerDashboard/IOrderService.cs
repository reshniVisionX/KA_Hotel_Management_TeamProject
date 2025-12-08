using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersByRestaurantAsync(int restaurantId, DateTime? startDate, DateTime? endDate, string? status);
        Task<IEnumerable<OrderDto>> GetTodayOrdersAsync(int restaurantId);
        Task<OrderSummaryDto> GetOrderSummaryAsync(int restaurantId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<DailyRevenueDto>> GetDailyRevenueAsync(int restaurantId);
    }
}