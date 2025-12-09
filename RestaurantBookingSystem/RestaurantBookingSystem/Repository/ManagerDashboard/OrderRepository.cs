using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookingContext _context;

        public OrderRepository(BookingContext context)
        {
            _context = context;
        }



        public async Task<IEnumerable<Orders>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Orders
                .Where(o => o.RestaurantId == restaurantId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Orders>> GetByRestaurantAndDateRangeAsync(int restaurantId, DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.RestaurantId == restaurantId && 
                           o.OrderDate >= startDate && 
                           o.OrderDate <= endDate)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Orders>> GetByRestaurantAndStatusAsync(int restaurantId, OrderStatus status)
        {
            return await _context.Orders
                .Where(o => o.RestaurantId == restaurantId && o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }



        public async Task<object> GetOrderSummaryAsync(int restaurantId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Orders
                .Where(o => o.RestaurantId == restaurantId);

            if (startDate.HasValue)
                query = query.Where(o => o.OrderDate >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(o => o.OrderDate <= endDate.Value);

            var orders = await query.ToListAsync();

            return new
            {
                TotalOrders = orders.Count,
                TotalRevenue = orders.Sum(o => o.TotalAmount),
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                ActiveOrders = orders.Count(o => o.Status == OrderStatus.InProgress),
                CompletedOrders = orders.Count(o => o.Status == OrderStatus.Completed),
                CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0
            };
        }

        public async Task<IEnumerable<Orders>> GetTodayOrdersAsync(int restaurantId)
        {
            var today = DateTime.Today;
            return await _context.Orders
                .Where(o => o.RestaurantId == restaurantId && o.OrderDate.Date == today)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}