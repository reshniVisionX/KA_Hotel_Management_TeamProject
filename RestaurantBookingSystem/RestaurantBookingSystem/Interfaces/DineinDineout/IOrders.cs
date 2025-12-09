using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IOrders
    {
        Task<IEnumerable<Orders>> GetAllOrdersAsync();
        Task<Orders?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Orders>> GetOrdersByUserAsync(int userId);
        Task<IEnumerable<Orders>> GetOrdersByRestaurantAsync(int restaurantId);
        Task<Orders> CreateOrderAsync(Orders order);
        Task<Orders?> UpdateOrderAsync(Orders order);
        Task<bool> RestaurantExistsAsync(int restaurantId);
        Task<List<CartItem>> GetUserCartItemsAsync(int userId, int restaurantId);
        Task<List<CartItem>> GetAllUserCartItemsAsync(int userId);
        Task<MenuList> GetMenuItemAsync(int itemId);
        Task<bool> ClearUserCartAsync(int userId, int restaurantId);
        Task<bool> ClearAllUserCartAsync(int userId);
        Task<bool> CancelUserOrdersAsync(int userId);
        Task<Orders?> GetUserPendingOrderAsync(int userId);
        Task<Restaurants?> GetRestaurantAsync(int restaurantId);
        Task<bool> ReduceMenuItemQuantityAsync(int itemId, int quantity);
    }

    public class CartItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
