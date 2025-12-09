using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuListController : ControllerBase
    {
        private readonly MenuListService _service;

        public MenuListController(MenuListService service)
        {
            _service = service;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult> GetMenuByRestaurantId(int restaurantId)
        {
            var items = await _service.GetMenuByRestaurantIdAsync(restaurantId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Menu items retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMenuItemById(int id)
        {
            var item = await _service.GetMenuItemByIdAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Menu item retrieved successfully",
                Data = item
            });
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchMenuByName([FromQuery] string name)
        {
            var items = await _service.SearchMenuByNameAsync(name);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Menu search completed successfully",
                Data = items
            });
        }
    }
}
