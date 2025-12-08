using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryService _service;

        public DeliveryController(DeliveryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeliveryCreateDto dto)
        {
            var result = await _service.CreateDeliveryAsync(dto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery created successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetDeliveryByIdAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery retrieved successfully",
                Data = result
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var result = await _service.GetDeliveriesByUserAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Deliveries retrieved successfully",
                Data = result
            });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, DeliveryStatusUpdateDto dto)
        {
            var result = await _service.UpdateDeliveryStatusAsync(id, dto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery status updated successfully",
                Data = result
            });
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteDelivery(DeliveryCompletionDto dto)
        {
            var result = await _service.CompleteDeliveryAsync(dto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery completed successfully",
                Data = result
            });
        }
    }
}
