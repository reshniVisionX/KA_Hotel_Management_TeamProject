using RestaurantBookingSystem.DTO;

namespace RestaurantBookingSystem.Interface.IService
{
    public interface IWishlistService
    {
        Task<List<WishlistDto>> GetUserWishlistAsync(int userId);
        Task<bool> AddToWishlistAsync(CreateWishlistDto createDto);
        Task<bool> RemoveFromWishlistAsync(int wishlistId);
        Task<bool> RemoveFromWishlistAsync(int userId, int itemId);
    }
}