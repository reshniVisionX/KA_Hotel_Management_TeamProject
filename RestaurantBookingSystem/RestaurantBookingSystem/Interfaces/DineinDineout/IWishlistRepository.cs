using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IWishlistRepository
    {
        Task<Wishlist> AddToWishlistAsync(Wishlist wishlist);
        Task<bool> RemoveFromWishlistAsync(int wishlistId);
        Task<List<Wishlist>> GetUserWishlistAsync(int userId);
        Task<Wishlist?> CheckItemInWishlistAsync(int userId, int itemId);
        Task<WishlistResponseDto> MapToResponseDtoAsync(Wishlist wishlist);
        Task<List<WishlistResponseDto>> GetUserWishlistWithDetailsAsync(int userId);
    }
}