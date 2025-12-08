using RestaurantBookingSystem.DTO;

namespace RestaurantBookingSystem.Interface.IService
{
    public interface IOrderHistoryService
    {
        Task<List<OrderHistoryDto>> GetAllOrdersAsync();
        Task<List<OrderHistoryDto>> GetOrdersByRestaurantAsync(int restaurantId);
        Task<List<OrderHistoryDto>> GetOrdersByUserAsync(int userId);
        Task<OrderHistoryDto?> GetOrderAsync(int orderId);
    }
}