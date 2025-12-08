using RestaurantBookingSystem.DTOs;

namespace RestaurantBookingSystem.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDto> AddToCartAsync(int userId, CartRequestDto request);
        Task<CartResponseDto?> UpdateCartItemAsync(int cartId, CartUpdateDto request);
        Task<bool> RemoveFromCartAsync(int cartId);
        Task<CartSummaryDto> GetUserCartAsync(int userId);
        Task<bool> ClearCartAsync(int userId);
        Task<CartTotalDto> GetCartTotalAsync(int userId);
        Task<CartItemExistsDto> IsItemInCartAsync(CheckCartItemDto request);
    }
}