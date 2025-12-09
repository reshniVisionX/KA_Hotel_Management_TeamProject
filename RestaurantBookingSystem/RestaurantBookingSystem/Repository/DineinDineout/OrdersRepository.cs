using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Repository
{
    public class OrdersRepository : IOrders
    {
        private readonly BookingContext _context;
        public OrdersRepository(BookingContext context)
        {
            _context = context;
        }
        public async Task<Orders> CreateOrderAsync(Orders order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Orders>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<Orders?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                  .Include(o => o.User)
                  .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Orders>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            return await _context.Orders
               .Where(o => o.RestaurantId == restaurantId)
               .Include(o => o.User)
               .ToListAsync();
        }

        public async Task<IEnumerable<Orders>> GetOrdersByUserAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<Orders?> UpdateOrderAsync(Orders order)
        {
            var existing = await _context.Orders.FindAsync(order.OrderId);
            if (existing == null)
                return null;

            _context.Entry(existing).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> RestaurantExistsAsync(int restaurantId)
        {
            return await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
        }

        public async Task<List<CartItem>> GetUserCartItemsAsync(int userId, int restaurantId)
        {
            return await _context.Cart
                .Where(c => c.UserId == userId && c.RestaurantId == restaurantId)
                .Select(c => new CartItem { ItemId = c.ItemId, Quantity = c.Quantity })
                .ToListAsync();
        }

        public async Task<MenuList> GetMenuItemAsync(int itemId)
        {
            var item = await _context.MenuList.FindAsync(itemId);
            if (item == null)
                throw new KeyNotFoundException($"Menu item with ID {itemId} not found");
            return item;
        }

        public async Task<List<CartItem>> GetAllUserCartItemsAsync(int userId)
        {
            return await _context.Cart
                .Where(c => c.UserId == userId)
                .Select(c => new CartItem { ItemId = c.ItemId, Quantity = c.Quantity })
                .ToListAsync();
        }

        public async Task<bool> ClearUserCartAsync(int userId, int restaurantId)
        {
            var cartItems = await _context.Cart
                .Where(c => c.UserId == userId && c.RestaurantId == restaurantId)
                .ToListAsync();
            
            if (cartItems.Any())
            {
                _context.Cart.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> ClearAllUserCartAsync(int userId)
        {
            var cartItems = await _context.Cart
                .Where(c => c.UserId == userId)
                .ToListAsync();
            
            if (cartItems.Any())
            {
                _context.Cart.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> CancelUserOrdersAsync(int userId)
        {
            var pendingOrders = await _context.Orders
                .Where(o => o.UserId == userId && o.Status == OrderStatus.Pending)
                .ToListAsync();
            
            if (pendingOrders.Any())
            {
                foreach (var order in pendingOrders)
                {
                    order.Status = OrderStatus.Cancelled;
                }
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Orders?> GetUserPendingOrderAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId && o.Status == OrderStatus.Pending)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();
        }

        public async Task<Restaurants?> GetRestaurantAsync(int restaurantId)
        {
            return await _context.Restaurants.FindAsync(restaurantId);
        }

        public async Task<bool> ReduceMenuItemQuantityAsync(int itemId, int quantity)
        {
            var menuItem = await _context.MenuList.FindAsync(itemId);
            if (menuItem == null) return false;

            if (menuItem.AvailableQty >= quantity)
            {
                menuItem.AvailableQty -= quantity;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
