using RestaurantBookingSystem.DTOorder;

namespace RestaurantBookingSystem.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDto> AddToCartAsync(int userId, CartRequestDto request);
        Task<CartResponseDto> UpdateCartItemAsync(int userId, int itemId, CartUpdateDto request);
        Task<bool> RemoveFromCartAsync(int cartId);
        Task<CartSummaryDto> GetUserCartAsync(int userId);
        Task<bool> ClearCartAsync(int userId);
        Task<CartResponseDto> IncrementQuantityAsync(int cartId);
        Task<CartResponseDto> DecrementQuantityAsync(int cartId);


    }
}