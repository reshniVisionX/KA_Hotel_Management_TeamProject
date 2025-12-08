using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces.IService;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // ---------------------------------------------------------
        // RESTAURANTS
        // ---------------------------------------------------------

        [HttpGet("restaurants")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var result = await _adminService.GetAllRestaurantsAsync();

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Restaurants retrieved successfully.",
                Data = result
            });
        }

        [HttpGet("restaurants/filter")]
        public async Task<IActionResult> FilterRestaurants(
            int? id,
            string? city,
            RestaurantCategory? category,
            FoodType? type,
            string? managerName)
        {
            var result = await _adminService.FilterRestaurants(id, city, category, type, managerName);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Restaurants filtered successfully.",
                Data = result
            });
        }

        [HttpPut("restaurants/{restaurantId}/toggle")]
        public async Task<IActionResult> ToggleRestaurantStatus(int restaurantId)
        {
            var success = await _adminService.ToggleRestaurantStatus(restaurantId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = success ? "Restaurant status updated successfully." : "Restaurant not found.",
                Data = null
            });
        }

        // ---------------------------------------------------------
        // MANAGERS
        // ---------------------------------------------------------

        [HttpGet("managers")]
        public async Task<IActionResult> GetAllManagers(int roleId)
        {
            var result = await _adminService.GetAllManagersAsync(roleId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Managers retrieved successfully.",
                Data = result
            });
        }

        [HttpPut("managers/{managerId}/toggle")]
        public async Task<IActionResult> ToggleManagerStatus(int managerId)
        {
            var success = await _adminService.ToggleManagerStatus(managerId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = success ? "Manager status updated successfully." : "Manager not found.",
                Data = null
            });
        }

        // ---------------------------------------------------------
        // ANALYTICS
        // ---------------------------------------------------------

        [HttpGet("analytics/dashboard")]
        public async Task<IActionResult> GetDashboardAnalytics()
        {
            var result = await _adminService.GetDashboardAnalyticsAsync();

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Dashboard analytics retrieved successfully.",
                Data = result
            });
        }

        [HttpGet("analytics/revenue")]
        public async Task<IActionResult> GetEntireRevenue([FromQuery] DateTime date)
        {
            var result = await _adminService.GetEntireRevenueAnalyticsAsync(date);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Revenue analytics retrieved successfully.",
                Data = result
            });
        }

        [HttpGet("analytics/restaurant/{restaurantId}")]
        public async Task<IActionResult> GetRestaurantRevenue(int restaurantId)
        {
            var result = await _adminService.GetRestaurantRevenueAsync(restaurantId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Restaurant revenue retrieved successfully.",
                Data = result
            });
        }
    }
}
