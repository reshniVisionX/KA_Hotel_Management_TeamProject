using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHistoryController : ControllerBase
    {
        private readonly IOrderHistoryService _orderHistoryService;

        public OrderHistoryController(IOrderHistoryService orderHistoryService)
        {
            _orderHistoryService = orderHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderHistoryService.GetAllOrdersAsync();
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Orders retrieved successfully",
                Data = result
            });
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetOrdersByRestaurant(int restaurantId)
        {
            var result = await _orderHistoryService.GetOrdersByRestaurantAsync(restaurantId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Orders retrieved successfully",
                Data = result
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var result = await _orderHistoryService.GetOrdersByUserAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Orders retrieved successfully",
                Data = result
            });
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var result = await _orderHistoryService.GetOrderAsync(orderId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Order retrieved successfully",
                Data = result
            });
        }
    }
}