using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryAddressController : ControllerBase
    {
        private readonly DeliveryAddressService _service;

        public DeliveryAddressController(DeliveryAddressService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetAddresses(int userId)
        {
            var result = await _service.GetUserAddressesAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Addresses retrieved successfully",
                Data = result
            });
        }

        [HttpGet("{addressId}")]
        public async Task<ActionResult> GetAddress(int addressId)
        {
            var result = await _service.GetAddressByIdAsync(addressId);
            if (result == null) throw new AppException("Address not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Address retrieved successfully",
                Data = result
            });
        }

        [HttpGet("default/{userId}")]
        public async Task<ActionResult> GetDefaultAddress(int userId)
        {
            var result = await _service.GetDefaultAddressAsync(userId);
            if (result == null) throw new AppException("Default address not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Default address retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddAddress(DeliveryAddress address)
        {
            await _service.AddAddressAsync(address);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Address added successfully",
                Data = address
            });
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAddress(DeliveryAddress address)
        {
            await _service.UpdateAddressAsync(address);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Address updated successfully",
                Data = address
            });
        }

        [HttpDelete("{addressId}")]
        public async Task<ActionResult> DeleteAddress(int addressId)
        {
            var success = await _service.DeleteAddressAsync(addressId);
            if (!success) throw new AppException("Address not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Address deleted successfully",
                Data = null
            });
        }

        [HttpPut("default/{userId}/{addressId}")]
        public async Task<ActionResult> SetDefault(int userId, int addressId)
        {
            var success = await _service.SetDefaultAddressAsync(userId, addressId);
            if (!success) throw new AppException("Failed to set default address");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Default address set successfully",
                Data = null
            });
        }
    }
}
