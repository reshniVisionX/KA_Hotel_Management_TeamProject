using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Interface.IService;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    public class TableBookingController : BaseController
    {
        private readonly ITableBookingService _tableBookingService;

        public TableBookingController(ITableBookingService tableBookingService, ILogger<TableBookingController> logger) : base(logger)
        {
            _tableBookingService = tableBookingService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                return Ok(DTOs.ApiResponse<object>.ErrorResponse("User ID must be greater than 0"));
            }

            return await ExecuteAsync(() => _tableBookingService.GetBookingsByUserIdAsync(userId), "Bookings retrieved successfully");
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetBookingsByRestaurantId(int restaurantId)
        {
            if (restaurantId <= 0)
            {
                _logger.LogWarning("Invalid restaurant ID: {RestaurantId}", restaurantId);
                return Ok(DTOs.ApiResponse<object>.ErrorResponse("Restaurant ID must be greater than 0"));
            }

            return await ExecuteAsync(() => _tableBookingService.GetBookingsByRestaurantIdAsync(restaurantId), "Bookings retrieved successfully");
        }
    }
}