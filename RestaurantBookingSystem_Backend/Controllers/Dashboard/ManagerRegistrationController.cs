using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManagerRegistrationController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerRegistrationController(IManagerService managerService)
        {
            _managerService = managerService;
        }


        [HttpPost("RegisterManagerWithRestaurant")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RegisterManagerWithRestaurant([FromForm] ManagerRegisterDTO dto)
        {
            if (!ModelState.IsValid)
                throw new AppException("Invalid data provided");

            var result = await _managerService.RegisterManagerWithRestaurantFormAsync(dto);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Manager and Restaurant registered successfully",
                Data = result
            });
        }

    }
}
