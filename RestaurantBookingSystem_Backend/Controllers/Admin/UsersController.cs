using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestaurantBookingSystem.DTOs.Admin;
using RestaurantBookingSystem.Services.Admin;
using RestaurantBookingSystem.Utils;
using System.Text;

namespace RestaurantBookingSystem.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserServices _service;
        private readonly TokenService _tokenService;

        public UsersController(UserServices service, IConfiguration configuration, TokenService tokenService)
        {
            _service = service;
            _tokenService = tokenService;
        }

       
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO dto)
        {
            var message = await _service.RegisterUserAsync(dto);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = message,
                Data = null
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _service.LoginAsync(dto);
            var token = _tokenService.GenerateToken(user);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Login successful",
                Data = new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Role.RoleName,
                    user.Mobile,
                    user.LastLogin,
                    Token = token
                }
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsersAsync();

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Users retrieved successfully",
                Data = users
            });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "User retrieved successfully",
                Data = user
            });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("user-active/{userId}")]
        public async Task<ActionResult> ToggleActiveStatus(int userId)
        {
            await _service.ToggleUserActiveStatusAsync(userId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "User active status toggled successfully",
                Data = null
            });
        }


        [HttpPost("generate-otp")]
        public IActionResult GenerateOTP([FromBody] OTPRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.MobileNo))
                return Ok(new ApiErrorResponse
                {
                    Success = false,
                    Message = "Mobile number is required."
                });

            var otp = _service.GenerateOtp(request.MobileNo);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "OTP sent successfully",
                Data = new { mobileNo = request.MobileNo, otp }
            });
        }


    }
}
