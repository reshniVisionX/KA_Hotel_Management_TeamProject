using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IService;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    public class UserProfileController : BaseController
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService, ILogger<UserProfileController> logger) : base(logger)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfiles()
        {
            return await ExecuteAsync(() => _userProfileService.GetAllProfilesAsync(), "Profiles retrieved successfully");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                return Ok(DTOs.ApiResponse<object>.ErrorResponse("User ID must be greater than 0"));
            }

            return await ExecuteAsync(() => _userProfileService.GetProfileAsync(userId), "Profile retrieved successfully");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateUserProfileDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for profile creation");
                return Ok(DTOs.ApiResponse.Error("Invalid data provided"));
            }

            return await ExecuteAsync(() => _userProfileService.CreateProfileAsync(createDto), "Profile created successfully", "Failed to create profile");
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateUserProfileDto updateDto)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                return Ok(DTOs.ApiResponse.Error("User ID must be greater than 0"));
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for profile update");
                return Ok(DTOs.ApiResponse.Error("Invalid data provided"));
            }

            return await ExecuteAsync(() => _userProfileService.UpdateProfileAsync(userId, updateDto), "Profile updated successfully", "User not found or update failed");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteProfile(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                return Ok(DTOs.ApiResponse.Error("User ID must be greater than 0"));
            }

            return await ExecuteAsync(() => _userProfileService.DeleteProfileAsync(userId), "Profile deleted successfully", "User not found");
        }
    }
}
