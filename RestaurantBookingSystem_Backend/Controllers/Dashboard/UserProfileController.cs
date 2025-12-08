using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfiles()
        {
            var result = await _userProfileService.GetAllProfilesAsync();
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Profiles retrieved successfully",
                Data = result
            });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            if (userId <= 0)
                throw new AppException("User ID must be greater than 0");

            var result = await _userProfileService.GetProfileAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Profile retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateUserProfileDto createDto)
        {
            if (!ModelState.IsValid)
                throw new AppException("Invalid data provided");

            var result = await _userProfileService.CreateProfileAsync(createDto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Profile created successfully",
                Data = result
            });
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateUserProfileDto updateDto)
        {
            if (userId <= 0)
                throw new AppException("User ID must be greater than 0");

            if (!ModelState.IsValid)
                throw new AppException("Invalid data provided");

            var result = await _userProfileService.UpdateProfileAsync(userId, updateDto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Profile updated successfully",
                Data = result
            });
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteProfile(int userId)
        {
            if (userId <= 0)
                throw new AppException("User ID must be greater than 0");

            var result = await _userProfileService.DeleteProfileAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Profile deleted successfully",
                Data = result
            });
        }
    }
}
