using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs.Admin;
using RestaurantBookingSystem.Interfaces.Admin;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers.Admin
{
    [Route("api/delivery/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminDeliveryController : ControllerBase
    {
        private readonly IAdminDeliveryService _deliveryService;

        public AdminDeliveryController(IAdminDeliveryService deliveryService )
        {
            _deliveryService = deliveryService;        
        }

        [HttpGet("forUser/{userId}")]
        public async Task<ActionResult> GetUserDeliveryAddresses(int userId)
        {
            var addresses = await _deliveryService.GetUserAddresses(userId);

            return Ok(new ApiSuccessResponse<object>
            {
                Success = true,
                Data = addresses
            });
        }

        [HttpPost("createAddress")]
        public async Task<ActionResult> AddAddress([FromBody] DeliveryAddress address)
        {
            var result = await _deliveryService.AddAddress(address);

            return Ok(new ApiSuccessResponse<object>
            {
                Success = true,
                Message = "Address created successfully.",
                Data = result
            });
        }

        [HttpPost("changeDefault")]
        public async Task<ActionResult> ChangeDefaultAddress([FromBody] ChangeDeliveryAddressDTO dto)
        {
            var result = await _deliveryService.ChangeDefaultAddress(dto.UserId, dto.DeliveryAddressId);
            return Ok(new ApiSuccessResponse<object>
            {
                Success = true,
                Message = "Default delivery address updated successfully.",
            });
        }
        [HttpGet("get-all-menu")]
        public async Task<IActionResult> GetAllMenuList()
        {
            var menuItems = await _deliveryService.GetAllMenuListAsync();

            return Ok(new ApiSuccessResponse<object>
            {
                Success = true,
                Message = "Menu list retrieved successfully",
                Data = menuItems
            });
        }

        [HttpPatch("complete-delivery/{deliveryId}")]
        public async Task<IActionResult> CompleteDelivery(int deliveryId)
        {
            await _deliveryService.CompleteDeliveryAsync(deliveryId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery completed successfully",
                Data = new { deliveryId }
            });
        }

        [HttpGet("person/{deliveryPersonId}")]
        public async Task<IActionResult> GetDeliveryHistory(int deliveryPersonId)
        {
            var deliveries = await _deliveryService.GetDeliveriesForPersonAsync(deliveryPersonId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Delivery details fetched successfully",
                Data = deliveries
            });
        }
    }
}


