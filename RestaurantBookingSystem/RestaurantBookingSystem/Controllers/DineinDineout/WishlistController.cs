using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Utils;

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
        public async Task<ActionResult> AddToWishlist(int userId, [FromBody] WishlistCreateDto request)
        {
            var result = await _service.AddToWishlistAsync(userId, request);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item added to wishlist successfully",
                Data = result
            });
        }

        [HttpDelete("remove/{wishlistId}")]
        public async Task<ActionResult> RemoveFromWishlist(int wishlistId)
        {
            var result = await _service.RemoveFromWishlistAsync(wishlistId);
            if (!result) throw new AppException("Wishlist item not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item removed from wishlist successfully",
                Data = null
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetUserWishlist(int userId)
        {
            var result = await _service.GetUserWishlistAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "User wishlist retrieved successfully",
                Data = result
            });
        }

        [HttpGet("check")]
        public async Task<ActionResult> CheckItemInWishlist([FromQuery] int userId, [FromQuery] int itemId)
        {
            var result = await _service.CheckItemInWishlistAsync(userId, itemId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Wishlist check completed successfully",
                Data = result
            });
        }
    }
}