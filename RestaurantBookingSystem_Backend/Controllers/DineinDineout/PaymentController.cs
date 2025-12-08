using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOorder;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _service;

        public PaymentController(PaymentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Payments retrieved successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) throw new AppException("Payment not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Payment retrieved successfully",
                Data = result
            });
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult> GetByOrder(int orderId)
        {
            var result = await _service.GetByOrderIdAsync(orderId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Order payments retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePaymentDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Payment created successfully",
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdatePaymentDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Payment updated successfully",
                Data = result
            });
        }
    }
}
