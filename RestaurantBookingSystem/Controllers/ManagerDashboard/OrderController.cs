using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult> GetOrdersByRestaurant(int restaurantId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? status)
        {
            var orders = await _service.GetOrdersByRestaurantAsync(restaurantId, startDate, endDate, status);
            return Ok(orders);
        }

        [HttpGet("restaurant/{restaurantId}/today")]
        public async Task<ActionResult> GetTodayOrders(int restaurantId)
        {
            var orders = await _service.GetTodayOrdersAsync(restaurantId);
            return Ok(orders);
        }

        [HttpGet("restaurant/{restaurantId}/summary")]
        public async Task<ActionResult> GetOrderSummary(int restaurantId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var summary = await _service.GetOrderSummaryAsync(restaurantId, startDate, endDate);
            return Ok(summary);
        }

        [HttpGet("restaurant/{restaurantId}/daily-revenue")]
        public async Task<ActionResult> GetDailyRevenue(int restaurantId)
        {
            var dailyRevenue = await _service.GetDailyRevenueAsync(restaurantId);
            return Ok(dailyRevenue);
        }
    }
}