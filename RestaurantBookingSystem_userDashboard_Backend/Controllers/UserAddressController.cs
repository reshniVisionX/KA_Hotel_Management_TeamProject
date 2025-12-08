using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Interface.IService;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    public class UserAddressController : BaseController
    {
        private readonly IUserAddressService _userAddressService;

        public UserAddressController(IUserAddressService userAddressService, ILogger<UserAddressController> logger) : base(logger)
        {
            _userAddressService = userAddressService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAddressesByUserId(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                return Ok(DTOs.ApiResponse<object>.ErrorResponse("User ID must be greater than 0"));
            }

            return await ExecuteAsync(() => _userAddressService.GetAddressesByUserIdAsync(userId), "Addresses retrieved successfully");
        }
    }
}