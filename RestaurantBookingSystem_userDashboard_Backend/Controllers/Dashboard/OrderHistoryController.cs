using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Interface.IService;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    public class OrderHistoryController : BaseController
    {
        private readonly IOrderHistoryService _orderHistoryService;

        public OrderHistoryController(IOrderHistoryService orderHistoryService, ILogger<OrderHistoryController> logger) : base(logger)
        {
            _orderHistoryService = orderHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            return await ExecuteAsync(() => _orderHistoryService.GetAllOrdersAsync(), "Orders retrieved successfully");
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetOrdersByRestaurant(int restaurantId)
        {
            return await ExecuteAsync(() => _orderHistoryService.GetOrdersByRestaurantAsync(restaurantId), "Orders retrieved successfully");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            return await ExecuteAsync(() => _orderHistoryService.GetOrdersByUserAsync(userId), "Orders retrieved successfully");
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            return await ExecuteAsync(() => _orderHistoryService.GetOrderAsync(orderId), "Order retrieved successfully");
        }
    }
}