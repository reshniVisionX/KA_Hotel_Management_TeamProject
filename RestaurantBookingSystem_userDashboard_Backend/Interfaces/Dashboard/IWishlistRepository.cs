using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Interface.IRepository
{
    public interface IWishlistRepository
    {
        Task<List<Wishlist>> GetUserWishlistAsync(int userId);
        Task<Wishlist> AddToWishlistAsync(Wishlist wishlist);
        Task<bool> RemoveFromWishlistAsync(int wishlistId);
        Task<bool> RemoveFromWishlistAsync(int userId, int itemId);
        Task<bool> ExistsAsync(int userId, int itemId);
    }
}