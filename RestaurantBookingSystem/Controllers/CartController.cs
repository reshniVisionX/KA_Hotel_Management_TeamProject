using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;

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
        public async Task<ActionResult<CartResponseDto>> AddToCart([FromBody] CartRequestDto request, [FromQuery] int userId)
        {
            try
            {
                var result = await _service.AddToCartAsync(userId, request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CartSummaryDto>> GetUserCart(int userId)
        {
            var result = await _service.GetUserCartAsync(userId);
            return Ok(result);
        }

        [HttpPut("update/{cartId}")]
        public async Task<ActionResult<CartResponseDto>> UpdateCartItem(int cartId, [FromBody] CartUpdateDto request)
        {
            var result = await _service.UpdateCartItemAsync(cartId, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("remove/{cartId}")]
        public async Task<ActionResult> RemoveFromCart(int cartId)
        {
            var result = await _service.RemoveFromCartAsync(cartId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("clear/{userId}")]
        public async Task<ActionResult> ClearCart(int userId)
        {
            var result = await _service.ClearCartAsync(userId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("total/{userId}")]
        public async Task<ActionResult<CartTotalDto>> GetCartTotal(int userId)
        {
            var result = await _service.GetCartTotalAsync(userId);
            return Ok(result);
        }

        [HttpGet("isInCart")]
        public async Task<ActionResult<CartItemExistsDto>> CheckItemInCart([FromQuery] CheckCartItemDto request)
        {
            var result = await _service.IsItemInCartAsync(request);
            return Ok(result);
        }
    }
}