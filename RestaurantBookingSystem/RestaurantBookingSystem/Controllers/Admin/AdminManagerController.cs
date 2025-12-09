using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces.IService;
using RestaurantBookingSystem.Model.Manager;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminManagerController : ControllerBase
    {
        private readonly IManagerRequestService _service;

        public AdminManagerController(IManagerRequestService service)
        {
            _service = service;
        }

        // ------------------- PAYOUTS -------------------
        [HttpPost("payout")]
        public async Task<IActionResult> ProcessPayout([FromBody] PayoutDTO payout)
        {
            var success = await _service.ProcessMonthlyPayoutToManagersAsync(payout);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = success ? "Payout processed successfully." : "Failed to process payout.",
                Data = null
            });
        }

        [HttpGet("payout-history/{managerId}")]
        public async Task<IActionResult> GetPayoutHistory(int managerId)
        {
            var result = await _service.GetPayoutHistoryAsync(managerId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Payout history retrieved successfully.",
                Data = result
            });
        }

        // ------------------- MANAGER VERIFICATION -------------------
        [HttpGet("unverified")]
        public async Task<IActionResult> GetUnverifiedManagers()
        {
            var result = await _service.GetAllUnverifiedManagersAsync();

            if (result == null || !result.Any())
            {
                return Ok(new ApiSuccessResponse<object>
                {
                    Message = "No unverified managers found.",
                    Data = Array.Empty<object>()
                });
            }

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Unverified managers retrieved successfully.",
                Data = result
            });
        }

        [HttpPut("verify/{managerId}")]
        public async Task<IActionResult> VerifyManager(int managerId, [FromQuery] bool isVerified)
        {
            var success = await _service.VerifyManagerAsync(managerId, isVerified);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = success
                    ? isVerified ? "Manager approved." : "Manager rejected."
                    : "Manager not found.",
                Data = null
            });
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterManagers(
            [FromQuery] bool isActive,
            [FromQuery] IsVerified? verification)
        {
            var result = await _service.FilterManagersAsync(isActive, verification);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Managers filtered successfully.",
                Data = result
            });
        }
    }
}
