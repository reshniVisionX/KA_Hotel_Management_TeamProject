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
    public class UserPreferenceController : ControllerBase
    {
        private readonly IUserPreferenceService _userPreferenceService;

        public UserPreferenceController(IUserPreferenceService userPreferenceService)
        {
            _userPreferenceService = userPreferenceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPreferences()
        {
            var result = await _userPreferenceService.GetAllPreferencesAsync();
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Preferences retrieved successfully",
                Data = result
            });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPreference(int userId)
        {
            var result = await _userPreferenceService.GetPreferenceAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Preference retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePreference([FromBody] CreateUserPreferenceDto createDto)
        {
            if (!ModelState.IsValid)
                throw new AppException("Invalid data provided");

            var result = await _userPreferenceService.CreatePreferenceAsync(createDto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Preference created successfully",
                Data = result
            });
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdatePreference(int userId, [FromBody] UpdateUserPreferenceDto updateDto)
        {
            if (!ModelState.IsValid)
                throw new AppException("Invalid data provided");

            var result = await _userPreferenceService.UpdatePreferenceAsync(userId, updateDto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Preference updated successfully",
                Data = result
            });
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeletePreference(int userId)
        {
            var result = await _userPreferenceService.DeletePreferenceAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Preference deleted successfully",
                Data = result
            });
        }
    }
}
