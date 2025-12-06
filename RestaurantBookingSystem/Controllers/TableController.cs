using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _service;

        public TableController(ITableService service)
        {
            _service = service;
        }

        [HttpGet("available/{restaurantId}")]
        public async Task<ActionResult<List<TableResponseDto>>> GetAvailableTables(int restaurantId)
        {
            var result = await _service.GetAvailableTablesAsync(restaurantId);
            return Ok(result);
        }
    }
}