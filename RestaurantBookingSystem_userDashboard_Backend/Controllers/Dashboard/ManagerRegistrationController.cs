using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    public class ManagerRegistrationController : BaseController
    {
        private readonly IManagerService _managerService;

        public ManagerRegistrationController(IManagerService managerService, ILogger<ManagerRegistrationController> logger) : base(logger)
        {
            _managerService = managerService;
        }

        [HttpPost("RegisterManagerWithRestaurant")]
        public async Task<IActionResult> RegisterManagerWithRestaurant([FromBody] ManagerRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for manager registration");
                return Ok(DTOs.ApiResponse.Error("Invalid data provided"));
            }

            return await ExecuteAsync(() => _managerService.RegisterManagerWithRestaurantAsync(dto), "Manager and Restaurant registered successfully");
        }
    }
}
