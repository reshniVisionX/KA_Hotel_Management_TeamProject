using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> AddToCartAsync(Cart cart);
        Task<Cart> UpdateCartItemAsync(Cart cart);
        Task<bool> RemoveFromCartAsync(int cartId);
        Task<List<Cart>> GetUserCartAsync(int userId);
        Task<Cart> GetCartItemAsync(int userId, int itemId);
        Task<Cart> GetCartItemByIdAsync(int cartId);
        Task<bool> ClearCartAsync(int userId);
        Task<MenuList?> GetMenuItemAsync(int itemId);
        Task<Restaurants?> GetRestaurantAsync(int restaurantId);
    }
}