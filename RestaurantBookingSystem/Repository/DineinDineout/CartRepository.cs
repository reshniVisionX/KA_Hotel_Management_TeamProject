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

        public async Task<Cart?> GetCartItemAsync(int userId, int itemId, int restaurantId)
        {
            return await _context.Cart
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == itemId && c.RestaurantId == restaurantId);
        }

        public async Task<Cart> UpdateCartItemAsync(Cart cart)
        {
            _context.Cart.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart?> GetCartByIdAsync(int cartId)
        {
            return await _context.Cart.FindAsync(cartId);
        }

        public async Task<bool> RemoveFromCartAsync(int cartId)
        {
            var cartItem = await GetCartByIdAsync(cartId);
            if (cartItem == null) return false;

            _context.Cart.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Cart>> GetUserCartAsync(int userId)
        {
            return await _context.Cart
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.AddedAt)
                .ToListAsync();
        }



        public async Task<bool> ClearCartAsync(int userId)
        {
            var cartItems = await _context.Cart.Where(c => c.UserId == userId).ToListAsync();
            if (!cartItems.Any()) return false;

            _context.Cart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CartItemExistsAsync(int userId, int itemId, int restaurantId)
        {
            return await _context.Cart
                .AnyAsync(c => c.UserId == userId && c.ItemId == itemId && c.RestaurantId == restaurantId);
        }

        public async Task<CartResponseDto> MapToResponseDtoAsync(Cart cart)
        {
            var menuItem = await _context.MenuList
                .FirstOrDefaultAsync(m => m.ItemId == cart.ItemId && m.RestaurantId == cart.RestaurantId);
            var restaurant = await _context.Restaurants.FindAsync(cart.RestaurantId);

            return new CartResponseDto
            {
                CartId = cart.CartId,
                ItemId = cart.ItemId,
                RestaurantId = cart.RestaurantId,
                ItemName = menuItem?.ItemName ?? "Item Not Found",
                RestaurantName = restaurant?.RestaurantName ?? "Restaurant Not Found",
                Price = menuItem?.Price ?? 0,
                Quantity = cart.Quantity,
                TotalPrice = (menuItem?.Price ?? 0) * cart.Quantity,
                IsVegetarian = menuItem?.IsVegetarian ?? false,
                AddedAt = cart.AddedAt
            };
        }

        public async Task<CartSummaryDto> GetUserCartSummaryAsync(int userId)
        {
            var cartItems = await _context.Cart
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.AddedAt)
                .ToListAsync();

            var responseItems = new List<CartResponseDto>();
            decimal grandTotal = 0;
            int totalItems = 0;
            int totalQuantity = 0;

            foreach (var item in cartItems)
            {
                var responseDto = await MapToResponseDtoAsync(item);
                responseItems.Add(responseDto);
                grandTotal += responseDto.TotalPrice;
                totalItems++;
                totalQuantity += item.Quantity;
            }

            return new CartSummaryDto
            {
                Items = responseItems,
                GrandTotal = grandTotal,
                TotalItems = totalItems,
                TotalQuantity = totalQuantity
            };
        }

        public async Task<CartTotalDto> GetCartTotalAsync(int userId)
        {
            var cartItems = await _context.Cart
                .Where(c => c.UserId == userId)
                .ToListAsync();

            decimal totalPrice = 0;
            int totalItems = cartItems.Count;
            int totalQuantity = 0;

            foreach (var item in cartItems)
            {
                var menuItem = await _context.MenuList
                    .FirstOrDefaultAsync(m => m.ItemId == item.ItemId && m.RestaurantId == item.RestaurantId);
                
                if (menuItem != null)
                {
                    totalPrice += menuItem.Price * item.Quantity;
                }
                totalQuantity += item.Quantity;
            }

            return new CartTotalDto
            {
                TotalPrice = totalPrice,
                TotalItems = totalItems,
                TotalQuantity = totalQuantity
            };
        }


    }
}