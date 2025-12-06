using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _service;

        public WishlistController(IWishlistService service)
        {
            _service = service;
        }

        [HttpPost("add/{userId}")]
        public async Task<ActionResult<WishlistResponseDto>> AddToWishlist(int userId, [FromBody] WishlistCreateDto request)
        {
            try
            {
                var result = await _service.AddToWishlistAsync(userId, request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("remove/{wishlistId}")]
        public async Task<ActionResult> RemoveFromWishlist(int wishlistId)
        {
            var result = await _service.RemoveFromWishlistAsync(wishlistId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<WishlistResponseDto>>> GetUserWishlist(int userId)
        {
            var result = await _service.GetUserWishlistAsync(userId);
            return Ok(result);
        }

        [HttpGet("check")]
        public async Task<ActionResult<WishlistCheckDto>> CheckItemInWishlist([FromQuery] int userId, [FromQuery] int itemId)
        {
            var result = await _service.CheckItemInWishlistAsync(userId, itemId);
            return Ok(result);
        }
    }
}