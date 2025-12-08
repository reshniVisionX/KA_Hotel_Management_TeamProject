using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> AddToCartAsync(Cart cart);
        Task<Cart?> GetCartByIdAsync(int cartId);
        Task<Cart?> GetCartItemAsync(int userId, int itemId, int restaurantId);
        Task<Cart> UpdateCartItemAsync(Cart cart);
        Task<bool> RemoveFromCartAsync(int cartId);
        Task<List<Cart>> GetUserCartAsync(int userId);
        Task<bool> ClearCartAsync(int userId);
        Task<bool> CartItemExistsAsync(int userId, int itemId, int restaurantId);
        Task<CartResponseDto> MapToResponseDtoAsync(Cart cart);
        Task<CartSummaryDto> GetUserCartSummaryAsync(int userId);
        Task<CartTotalDto> GetCartTotalAsync(int userId);
    }
}