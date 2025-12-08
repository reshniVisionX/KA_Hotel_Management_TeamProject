using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult> GetCustomersByRestaurant(int restaurantId)
        {
            var customers = await _service.GetCustomersByRestaurantAsync(restaurantId);
            return Ok(customers);
        }

        [HttpGet("restaurant/{restaurantId}/recent")]
        public async Task<ActionResult> GetRecentCustomers(int restaurantId, [FromQuery] int days = 30)
        {
            var customers = await _service.GetRecentCustomersAsync(restaurantId, days);
            return Ok(customers);
        }

        [HttpGet("restaurant/{restaurantId}/frequent")]
        public async Task<ActionResult> GetFrequentCustomers(int restaurantId, [FromQuery] int minOrders = 3)
        {
            var customers = await _service.GetFrequentCustomersAsync(restaurantId, minOrders);
            return Ok(customers);
        }

        [HttpGet("restaurant/{restaurantId}/summary")]
        public async Task<ActionResult> GetCustomerSummary(int restaurantId)
        {
            var summary = await _service.GetCustomerSummaryAsync(restaurantId);
            return Ok(summary);
        }
    }
}