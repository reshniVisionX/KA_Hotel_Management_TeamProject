using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepo;

        public WishlistService(IWishlistRepository wishlistRepo)
        {
            _wishlistRepo = wishlistRepo;
        }

        public async Task<List<WishlistDto>> GetUserWishlistAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            var wishlists = await _wishlistRepo.GetUserWishlistAsync(userId);
            return wishlists.Select(w => new WishlistDto
            {
                WishlistId = w.WishlistId,
                UserId = w.UserId,
                ItemId = w.ItemId,
                RestaurantId = w.RestaurantId,
                CreatedAt = w.CreatedAt,
                UserName = w.User != null ? $"{w.User.FirstName} {w.User.LastName}" : null
            }).ToList();
        }

        public async Task<bool> AddToWishlistAsync(CreateWishlistDto createDto)
        {
            if (createDto.UserId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            if (createDto.ItemId <= 0)
                throw new ArgumentException("Item ID must be greater than 0");

            if (createDto.RestaurantId <= 0)
                throw new ArgumentException("Restaurant ID must be greater than 0");

            // Check for duplicates
            var exists = await _wishlistRepo.ExistsAsync(createDto.UserId, createDto.ItemId);
            if (exists)
                throw new InvalidOperationException("Item already exists in wishlist");

            var wishlist = new Wishlist
            {
                UserId = createDto.UserId,
                ItemId = createDto.ItemId,
                RestaurantId = createDto.RestaurantId,
                CreatedAt = DateTime.Now
            };

            await _wishlistRepo.AddToWishlistAsync(wishlist);
            return true;
        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            if (wishlistId <= 0)
                throw new ArgumentException("Wishlist ID must be greater than 0");

            return await _wishlistRepo.RemoveFromWishlistAsync(wishlistId);
        }

        public async Task<bool> RemoveFromWishlistAsync(int userId, int itemId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            if (itemId <= 0)
                throw new ArgumentException("Item ID must be greater than 0");

            return await _wishlistRepo.RemoveFromWishlistAsync(userId, itemId);
        }
    }
}