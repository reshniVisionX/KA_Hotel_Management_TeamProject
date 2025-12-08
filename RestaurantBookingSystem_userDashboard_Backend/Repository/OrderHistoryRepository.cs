using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class OrderHistoryRepository : IOrderHistoryRepository
    {
        private readonly BookingContext _context;

        public OrderHistoryRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<List<Orders>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<List<Orders>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Where(o => o.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<List<Orders>> GetOrdersByUserAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<Orders?> GetOrderAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
    }
}