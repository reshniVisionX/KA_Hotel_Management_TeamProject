using RestaurantBookingSystem.DTOs;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IWishlistService
    {
        Task<WishlistResponseDto> AddToWishlistAsync(int userId, WishlistCreateDto request);
        Task<bool> RemoveFromWishlistAsync(int wishlistId);
        Task<List<WishlistResponseDto>> GetUserWishlistAsync(int userId);
        Task<WishlistCheckDto> CheckItemInWishlistAsync(int userId, int itemId);
    }
}