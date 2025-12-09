using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddToCart([FromBody] CartRequestDto request, [FromQuery] int userId)
        {
            var result = await _service.AddToCartAsync(userId, request);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item added to cart successfully",
                Data = result
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetUserCart(int userId)
        {
            var result = await _service.GetUserCartAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Cart retrieved successfully",
                Data = result
            });
        }

        [HttpPut("update/{cartId}")]
        public async Task<ActionResult> UpdateCartItem(int cartId, [FromBody] CartUpdateDto request)
        {
            var result = await _service.UpdateCartItemAsync(cartId, request);
            if (result == null) throw new AppException("Cart item not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Cart item updated successfully",
                Data = result
            });
        }

        [HttpDelete("remove/{cartId}")]
        public async Task<ActionResult> RemoveFromCart(int cartId)
        {
            var result = await _service.RemoveFromCartAsync(cartId);
            if (!result) throw new AppException("Cart item not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item removed from cart successfully",
                Data = null
            });
        }

        [HttpDelete("clear/{userId}")]
        public async Task<ActionResult> ClearCart(int userId)
        {
            var result = await _service.ClearCartAsync(userId);
            if (!result) throw new AppException("Cart not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Cart cleared successfully",
                Data = null
            });
        }

        [HttpGet("total/{userId}")]
        public async Task<ActionResult> GetCartTotal(int userId)
        {
            var result = await _service.GetCartTotalAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Cart total retrieved successfully",
                Data = result
            });
        }

        [HttpGet("isInCart")]
        public async Task<ActionResult> CheckItemInCart([FromQuery] CheckCartItemDto request)
        {
            var result = await _service.IsItemInCartAsync(request);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Cart item check completed",
                Data = result
            });
        }
    }
}