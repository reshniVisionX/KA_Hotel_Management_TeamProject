using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantService _service;

        public RestaurantController(RestaurantService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRestaurants()
        {
            var result = await _service.GetAllRestaurantsAsync();
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Restaurants retrieved successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRestaurantById(int id)
        {
            var restaurant = await _service.GetRestaurantByIdAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Restaurant retrieved successfully",
                Data = restaurant
            });
        }

        [HttpGet("{id}/menu")]
        public async Task<ActionResult> GetMenuByRestaurantId(int id)
        {
            var menu = await _service.GetMenuByRestaurantIdAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Menu retrieved successfully",
                Data = menu
            });
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchRestaurants(
          [FromQuery] string? name,
          [FromQuery] string? city)
        {
            var results = await _service.SearchRestaurantsAsync(name, city);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Search completed successfully",
                Data = results
            });
        }
    }
}
