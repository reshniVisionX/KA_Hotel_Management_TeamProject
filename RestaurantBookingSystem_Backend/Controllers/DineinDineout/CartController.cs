using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOorder;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpPost("add/{userId}")]
        public async Task<ActionResult> AddToCart(int userId, [FromBody] CartRequestDto request)
        {
            var result = await _service.AddToCartAsync(userId, request);

            return Ok(new ApiSuccessResponse<CartResponseDto>
            {
                Success = true,
                Message = "Item added to cart successfully",
                Data = result
            });
        }

        // Get Cart
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetCart(int userId)
        {
            var cart = await _service.GetUserCartAsync(userId);

            return Ok(new ApiSuccessResponse<CartSummaryDto>
            {
                Success = true,
                Message = "Cart retrieved successfully",
                Data = cart
            });
        }

        // Remove Item
        [HttpDelete("remove/{cartId}")]
        public async Task<ActionResult> RemoveItem(int cartId)
        {
            var removed = await _service.RemoveFromCartAsync(cartId);

            if (!removed)
                throw new AppException("Cart item not found");

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item removed successfully",
                Data = null
            });
        }

        // Clear Cart
        [HttpDelete("clear/{userId}")]
        public async Task<ActionResult> ClearCart(int userId)
        {
            var cleared = await _service.ClearCartAsync(userId);

            if (!cleared)
                throw new AppException("Cart not found");

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Cart cleared successfully",
                Data = null
            });
        }

        // Increment Quantity
        [HttpPatch("increment/{cartId}")]
        public async Task<ActionResult> IncrementQuantity(int cartId)
        {
            var result = await _service.IncrementQuantityAsync(cartId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Quantity increased successfully",
                Data = result
            });
        }

        // Decrement Quantity
        [HttpPatch("decrement/{cartId}")]
        public async Task<ActionResult> DecrementQuantity(int cartId)
        {
            var result = await _service.DecrementQuantityAsync(cartId);

            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Quantity decreased successfully",
                Data = result
            });
        }
    }
}