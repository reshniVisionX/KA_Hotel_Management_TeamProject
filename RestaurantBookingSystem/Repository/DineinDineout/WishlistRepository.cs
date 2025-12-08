using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly BookingContext _context;

        public WishlistRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<Wishlist> AddToWishlistAsync(Wishlist wishlist)
        {
            _context.Wishlist.Add(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            var wishlistItem = await _context.Wishlist.FindAsync(wishlistId);
            
            if (wishlistItem == null) return false;
            
            _context.Wishlist.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Wishlist>> GetUserWishlistAsync(int userId)
        {
            return await _context.Wishlist
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();
        }

        public async Task<Wishlist?> CheckItemInWishlistAsync(int userId, int itemId)
        {
            return await _context.Wishlist
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ItemId == itemId);
        }

        public async Task<WishlistResponseDto> MapToResponseDtoAsync(Wishlist wishlist)
        {
            var menuItem = await _context.MenuList.FindAsync(wishlist.ItemId);
            var restaurant = await _context.Restaurants.FindAsync(wishlist.RestaurantId);

            return new WishlistResponseDto
            {
                WishlistId = wishlist.WishlistId,
                ItemId = wishlist.ItemId,
                RestaurantId = wishlist.RestaurantId,
                ItemName = menuItem?.ItemName ?? "",
                RestaurantName = restaurant?.RestaurantName ?? "",
                Price = menuItem?.Price ?? 0,
                IsVegetarian = menuItem?.IsVegetarian ?? false,
                CreatedAt = wishlist.CreatedAt
            };
        }

        public async Task<List<WishlistResponseDto>> GetUserWishlistWithDetailsAsync(int userId)
        {
            var wishlistItems = await _context.Wishlist
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();

            var result = new List<WishlistResponseDto>();
            foreach (var item in wishlistItems)
            {
                result.Add(await MapToResponseDtoAsync(item));
            }

            return result;
        }


    }
}