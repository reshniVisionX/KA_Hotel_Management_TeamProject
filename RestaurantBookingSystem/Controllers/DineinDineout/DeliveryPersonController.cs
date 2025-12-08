using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryPersonController : ControllerBase
    {
        private readonly DeliveryPersonService _service;

        public DeliveryPersonController(DeliveryPersonService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery persons retrieved successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) throw new AppException("Delivery person not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery person retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDeliveryPersonDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery person created successfully",
                Data = created
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(DeliveryPersonDto dto)
        {
            var updated = await _service.UpdateAsync(dto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery person updated successfully",
                Data = updated
            });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var success = await _service.UpdateStatusAsync(id, status);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Status updated successfully",
                Data = null
            });
        }

        [HttpPost("{id}/generate-otp")]
        public async Task<IActionResult> GenerateOtp(int id)
        {
            var otp = await _service.GenerateOtpAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "OTP generated successfully",
                Data = new { otp }
            });
        }

        [HttpPost("{id}/complete-delivery")]
        public async Task<IActionResult> CompleteDelivery(int id)
        {
            await _service.CompleteDeliveryAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery statistics updated successfully",
                Data = null
            });
        }

        [HttpGet("{id}/stats")]
        public async Task<IActionResult> GetDeliveryStats(int id)
        {
            var stats = await _service.GetDeliveryStatsAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery stats retrieved successfully",
                Data = new { 
                    deliveryPersonId = id,
                    totalDeliveries = stats.totalDeliveries,
                    averageRating = stats.averageRating
                }
            });
        }
    }
}