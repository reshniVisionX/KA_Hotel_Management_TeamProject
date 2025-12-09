using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableBookingController : ControllerBase
    {
        private readonly ITableBookingService _tableBookingService;

        public TableBookingController(ITableBookingService tableBookingService)
        {
            _tableBookingService = tableBookingService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(int userId)
        {
            if (userId <= 0)
                throw new AppException("User ID must be greater than 0");

            var result = await _tableBookingService.GetBookingsByUserIdAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Bookings retrieved successfully",
                Data = result
            });
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetBookingsByRestaurantId(int restaurantId)
        {
            if (restaurantId <= 0)
                throw new AppException("Restaurant ID must be greater than 0");

            var result = await _tableBookingService.GetBookingsByRestaurantIdAsync(restaurantId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Bookings retrieved successfully",
                Data = result
            });
        }
    }
}