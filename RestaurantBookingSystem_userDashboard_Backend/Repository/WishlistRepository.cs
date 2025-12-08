using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Repository
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly BookingContext _context;

        public WishlistRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<List<Wishlist>> GetUserWishlistAsync(int userId)
        {
            return await _context.Wishlist
                .Include(w => w.User)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<Wishlist> AddToWishlistAsync(Wishlist wishlist)
        {
            _context.Wishlist.Add(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            var wishlist = await _context.Wishlist.FindAsync(wishlistId);
            if (wishlist == null) return false;

            _context.Wishlist.Remove(wishlist);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveFromWishlistAsync(int userId, int itemId)
        {
            var wishlist = await _context.Wishlist
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ItemId == itemId);

            if (wishlist == null) return false;

            _context.Wishlist.Remove(wishlist);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int userId, int itemId)
        {
            return await _context.Wishlist
                .AnyAsync(w => w.UserId == userId && w.ItemId == itemId);
        }
    }
}