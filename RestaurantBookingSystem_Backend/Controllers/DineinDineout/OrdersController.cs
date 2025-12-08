using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersService _service;

        public OrdersController(OrdersService service)
        {
            _service = service;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> CreateFromCart(int userId)
        {
            var result = await _service.CreateFromCartAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Order created successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) throw new AppException("Order not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Order retrieved successfully",
                Data = result
            });
        }

        [HttpGet("summary/{userId}")]
        public async Task<ActionResult> GetOrderSummary(int userId)
        {
            var summary = await _service.GetOrderSummaryAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Order summary retrieved successfully",
                Data = summary
            });
        }
    }
}
