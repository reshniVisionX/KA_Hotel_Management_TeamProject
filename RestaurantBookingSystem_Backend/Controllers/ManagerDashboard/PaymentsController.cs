using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult> GetPaymentsByRestaurant(int restaurantId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? paymentMethod)
        {
            var payments = await _service.GetPaymentsByRestaurantAsync(restaurantId, startDate, endDate, paymentMethod);
            return Ok(payments);
        }

        [HttpGet("restaurant/{restaurantId}/today")]
        public async Task<ActionResult> GetTodayPayments(int restaurantId)
        {
            var payments = await _service.GetTodayPaymentsAsync(restaurantId);
            return Ok(payments);
        }

        [HttpGet("restaurant/{restaurantId}/summary")]
        public async Task<ActionResult> GetPaymentSummary(int restaurantId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var summary = await _service.GetPaymentSummaryAsync(restaurantId, startDate, endDate);
            return Ok(summary);
        }
    }
}