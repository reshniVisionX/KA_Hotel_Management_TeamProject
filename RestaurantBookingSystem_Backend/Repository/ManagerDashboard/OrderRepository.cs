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

            var today = DateTime.Today;
            var todayOrders = orders.Where(o => o.OrderDate.Date == today).ToList();

            return new
            {
                TotalOrders = orders.Count,
                TodayOrders = todayOrders.Count,  // ADD THIS
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                CompletedOrders = orders.Count(o => o.Status == OrderStatus.Completed),
                TotalRevenue = orders.Sum(o => o.TotalAmount),
                TodayRevenue = todayOrders.Sum(o => o.TotalAmount)  // ADD THIS
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

        public async Task<IEnumerable<(DateTime Date, decimal Revenue)>> GetDailyRevenueAsync(int restaurantId)
        {
            var sevenDaysAgo = DateTime.Today.AddDays(-6);

            // First, let's get all orders for this restaurant to debug
            var allOrders = await _context.Orders
                .Where(o => o.RestaurantId == restaurantId)
                .ToListAsync();

            // Filter completed orders in the date range
            var completedOrders = allOrders
                .Where(o => o.OrderDate.Date >= sevenDaysAgo && o.Status == (OrderStatus)2)
                .ToList();

            // Group by date
            var dailyRevenue = completedOrders
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Sum(o => o.TotalAmount) })
                .ToList();

            // Create all 7 days and fill with actual data or 0
            var result = new List<(DateTime Date, decimal Revenue)>();
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Today.AddDays(-i);
                var revenue = dailyRevenue.FirstOrDefault(d => d.Date == date)?.Revenue ?? 0;
                result.Add((date, revenue));
            }

            return result;
        }
    }
}