using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IService;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    public class UserPreferenceController : BaseController
    {
        private readonly IUserPreferenceService _userPreferenceService;

        public UserPreferenceController(IUserPreferenceService userPreferenceService, ILogger<UserPreferenceController> logger) : base(logger)
        {
            _userPreferenceService = userPreferenceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPreferences()
        {
            return await ExecuteAsync(() => _userPreferenceService.GetAllPreferencesAsync(), "Preferences retrieved successfully");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPreference(int userId)
        {
            return await ExecuteAsync(() => _userPreferenceService.GetPreferenceAsync(userId), "Preference retrieved successfully");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePreference([FromBody] CreateUserPreferenceDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for preference creation");
                return Ok(DTOs.ApiResponse.Error("Invalid data provided"));
            }

            return await ExecuteAsync(() => _userPreferenceService.CreatePreferenceAsync(createDto), "Preference created successfully", "Failed to create preference");
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdatePreference(int userId, [FromBody] UpdateUserPreferenceDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for preference update");
                return Ok(DTOs.ApiResponse.Error("Invalid data provided"));
            }

            return await ExecuteAsync(() => _userPreferenceService.UpdatePreferenceAsync(userId, updateDto), "Preference updated successfully", "Preferences not found or update failed");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeletePreference(int userId)
        {
            return await ExecuteAsync(() => _userPreferenceService.DeletePreferenceAsync(userId), "Preference deleted successfully", "Preferences not found");
        }
    }
}
