using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressService _userAddressService;

        public UserAddressController(IUserAddressService userAddressService)
        {
            _userAddressService = userAddressService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAddressesByUserId(int userId)
        {
            if (userId <= 0)
                throw new AppException("User ID must be greater than 0");

            var result = await _userAddressService.GetAddressesByUserIdAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Addresses retrieved successfully",
                Data = result
            });
        }
    }
}