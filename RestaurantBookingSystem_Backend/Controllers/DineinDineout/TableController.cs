using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TableController : ControllerBase
    {
        private readonly ITableService _service;

        public TableController(ITableService service)
        {
            _service = service;
        }

        [HttpGet("available/{restaurantId}")]
        public async Task<ActionResult> GetAvailableTables(int restaurantId)
        {
            var result = await _service.GetAvailableTablesAsync(restaurantId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Available tables retrieved successfully",
                Data = result
            });
        }
    }
}