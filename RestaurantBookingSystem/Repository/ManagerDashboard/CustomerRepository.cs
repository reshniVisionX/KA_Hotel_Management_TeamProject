using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BookingContext _context;

        public CustomerRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users>> GetCustomersByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Users
                .Where(u => _context.Orders.Any(o => o.RestaurantId == restaurantId && o.UserId == u.UserId))
                .Include(u => u.Role)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Users>> GetRecentCustomersAsync(int restaurantId, int days = 30)
        {
            var cutoffDate = DateTime.Now.AddDays(-days);
            return await _context.Users
                .Where(u => _context.Orders.Any(o => o.RestaurantId == restaurantId && 
                                                   o.UserId == u.UserId && 
                                                   o.OrderDate >= cutoffDate))
                .Include(u => u.Role)
                .OrderByDescending(u => u.LastLogin)
                .ToListAsync();
        }

        public async Task<IEnumerable<Users>> GetFrequentCustomersAsync(int restaurantId, int minOrders = 3)
        {
            return await _context.Users
                .Where(u => _context.Orders.Count(o => o.RestaurantId == restaurantId && o.UserId == u.UserId) >= minOrders)
                .Include(u => u.Role)
                .OrderByDescending(u => _context.Orders.Count(o => o.RestaurantId == restaurantId && o.UserId == u.UserId))
                .ToListAsync();
        }

        public async Task<object> GetCustomerSummaryAsync(int restaurantId)
        {
            var totalCustomers = await _context.Users
                .CountAsync(u => _context.Orders.Any(o => o.RestaurantId == restaurantId && o.UserId == u.UserId));

            var recentCustomers = await _context.Users
                .CountAsync(u => _context.Orders.Any(o => o.RestaurantId == restaurantId && 
                                                        o.UserId == u.UserId && 
                                                        o.OrderDate >= DateTime.Now.AddDays(-30)));

            var frequentCustomers = await _context.Users
                .CountAsync(u => _context.Orders.Count(o => o.RestaurantId == restaurantId && o.UserId == u.UserId) >= 3);

            return new
            {
                TotalCustomers = totalCustomers,
                RecentCustomers = recentCustomers,
                FrequentCustomers = frequentCustomers,
                NewCustomersThisMonth = recentCustomers
            };
        }
    }
}