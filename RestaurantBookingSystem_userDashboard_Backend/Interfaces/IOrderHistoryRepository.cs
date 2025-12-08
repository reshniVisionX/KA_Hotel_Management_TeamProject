using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface.IRepository
{
    public interface IOrderHistoryRepository
    {
        Task<List<Orders>> GetAllOrdersAsync();
        Task<List<Orders>> GetOrdersByRestaurantAsync(int restaurantId);
        Task<List<Orders>> GetOrdersByUserAsync(int userId);
        Task<Orders?> GetOrderAsync(int orderId);
    }
}