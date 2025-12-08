using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly BookingContext _context;

        public CartRepository(BookingContext context)
        {
            _context = context;
        }
        public async Task<Cart> AddToCartAsync(Cart cart)
        {
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> UpdateCartItemAsync(Cart cart)
        {
            _context.Cart.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> RemoveFromCartAsync(int cartId)
        {
            var cartItem = await _context.Cart.FindAsync(cartId);

            if (cartItem == null) return false;

            _context.Cart.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Cart>> GetUserCartAsync(int userId)
        {
            return await _context.Cart
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.AddedAt)
                .ToListAsync();
        }

        public async Task<Cart> GetCartItemAsync(int userId, int itemId)
        {
            return await _context.Cart
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == itemId);
        }

        public async Task<Cart> GetCartItemByIdAsync(int cartId)
        {
            return await _context.Cart.FindAsync(cartId);
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cartItems = await _context.Cart
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any()) return false;

            _context.Cart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MenuList?> GetMenuItemAsync(int itemId)
        {
            return await _context.MenuList.FindAsync(itemId);
        }

        public async Task<Restaurants?> GetRestaurantAsync(int restaurantId)
        {
            return await _context.Restaurants.FindAsync(restaurantId);
        }
    }
}