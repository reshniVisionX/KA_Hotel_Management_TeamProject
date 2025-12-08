using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByRestaurantAsync(int restaurantId, DateTime? startDate, DateTime? endDate, string? status)
        {
            IEnumerable<Orders> orders;
            
            if (startDate.HasValue && endDate.HasValue)
            {
                orders = await _repo.GetByRestaurantAndDateRangeAsync(restaurantId, startDate.Value, endDate.Value);
            }
            else if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            {
                orders = await _repo.GetByRestaurantAndStatusAsync(restaurantId, orderStatus);
            }
            else
            {
                orders = await _repo.GetByRestaurantIdAsync(restaurantId);
            }

            return orders.Select(MapToDto);
        }

        public async Task<IEnumerable<OrderDto>> GetTodayOrdersAsync(int restaurantId)
        {
            var orders = await _repo.GetTodayOrdersAsync(restaurantId);
            return orders.Select(MapToDto);
        }

        public async Task<OrderSummaryDto> GetOrderSummaryAsync(int restaurantId, DateTime? startDate, DateTime? endDate)
        {
            var summary = await _repo.GetOrderSummaryAsync(restaurantId, startDate, endDate);
            return new OrderSummaryDto
            {
                TotalOrders = (int)((dynamic)summary).TotalOrders,
                TodayOrders = (int)((dynamic)summary).TodayOrders,
                PendingOrders = (int)((dynamic)summary).PendingOrders,
                CompletedOrders = (int)((dynamic)summary).CompletedOrders,
                TotalRevenue = (decimal)((dynamic)summary).TotalRevenue,
                TodayRevenue = (decimal)((dynamic)summary).TodayRevenue
            };
        }

        public async Task<IEnumerable<DailyRevenueDto>> GetDailyRevenueAsync(int restaurantId)
        {
            var dailyRevenue = await _repo.GetDailyRevenueAsync(restaurantId);
            return dailyRevenue.Select(d => new DailyRevenueDto
            {
                Date = d.Date,
                Revenue = d.Revenue
            });
        }

        private static OrderDto MapToDto(Orders o)
        {
            return new OrderDto
            {
                OrderId = o.OrderId,
                RestaurantId = o.RestaurantId,
                Items = System.Text.Json.JsonSerializer.Deserialize<List<ItemQuantity>>(o.ItemsList ?? "[]"),
                OrderType = o.OrderType,
                UserId = o.UserId,
                QtyOrdered = o.QtyOrdered,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status
            };
        }
    }
}