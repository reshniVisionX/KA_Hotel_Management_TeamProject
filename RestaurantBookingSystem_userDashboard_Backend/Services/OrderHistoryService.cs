using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Interface.IService;

namespace RestaurantBookingSystem.Services
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly IOrderHistoryRepository _orderRepo;

        public OrderHistoryService(IOrderHistoryRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<List<OrderHistoryDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            return orders.Select(o => new OrderHistoryDto
            {
                OrderId = o.OrderId,
                RestaurantId = o.RestaurantId,
                UserId = o.UserId,
                ItemsList = o.ItemsList,
                OrderType = o.OrderType,
                QtyOrdered = o.QtyOrdered,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                UserName = o.User != null ? $"{o.User.FirstName} {o.User.LastName}" : null
            }).ToList();
        }

        public async Task<List<OrderHistoryDto>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            if (restaurantId <= 0)
                throw new ArgumentException("Restaurant ID must be greater than 0");

            var orders = await _orderRepo.GetOrdersByRestaurantAsync(restaurantId);
            return orders.Select(o => new OrderHistoryDto
            {
                OrderId = o.OrderId,
                RestaurantId = o.RestaurantId,
                UserId = o.UserId,
                ItemsList = o.ItemsList,
                OrderType = o.OrderType,
                QtyOrdered = o.QtyOrdered,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                UserName = o.User != null ? $"{o.User.FirstName} {o.User.LastName}" : null
            }).ToList();
        }

        public async Task<List<OrderHistoryDto>> GetOrdersByUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            var orders = await _orderRepo.GetOrdersByUserAsync(userId);
            return orders.Select(o => new OrderHistoryDto
            {
                OrderId = o.OrderId,
                RestaurantId = o.RestaurantId,
                UserId = o.UserId,
                ItemsList = o.ItemsList,
                OrderType = o.OrderType,
                QtyOrdered = o.QtyOrdered,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                UserName = o.User != null ? $"{o.User.FirstName} {o.User.LastName}" : null
            }).ToList();
        }

        public async Task<OrderHistoryDto?> GetOrderAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Order ID must be greater than 0");

            var order = await _orderRepo.GetOrderAsync(orderId);
            if (order == null) return null;

            return new OrderHistoryDto
            {
                OrderId = order.OrderId,
                RestaurantId = order.RestaurantId,
                UserId = order.UserId,
                ItemsList = order.ItemsList,
                OrderType = order.OrderType,
                QtyOrdered = order.QtyOrdered,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                UserName = order.User != null ? $"{order.User.FirstName} {order.User.LastName}" : null
            };
        }
    }
}