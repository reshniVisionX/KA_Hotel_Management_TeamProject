using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _service;

        public RestaurantsController(IRestaurantService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetRestaurants()
        {
            var restaurants = await _service.GetAllAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var restaurant = await _service.GetByIdAsync(id);
            return restaurant == null ? NotFound() : Ok(restaurant);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateRestaurantsDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromQuery] bool isActive)
        {
            var result = await _service.UpdateStatusAsync(id, isActive);
            var message = isActive ? "Restaurant activated successfully" : "Restaurant deactivated successfully";
            return !result ? NotFound() : Ok(new { Message = message });
        }
    }
}