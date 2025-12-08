using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistsService _wishlistService;

        public WishlistsController(IWishlistsService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserWishlist(int userId)
        {
            var result = await _wishlistService.GetUserWishlistAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Wishlist retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] CreateWishlistDto createDto)
        {
            if (!ModelState.IsValid)
                throw new AppException("Invalid data provided");

            var result = await _wishlistService.AddToWishlistAsync(createDto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item added to wishlist successfully",
                Data = result
            });
        }

        [HttpDelete("{wishlistId}")]
        public async Task<IActionResult> RemoveFromWishlist(int wishlistId)
        {
            var result = await _wishlistService.RemoveFromWishlistAsync(wishlistId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item removed from wishlist successfully",
                Data = result
            });
        }

        [HttpDelete("user/{userId}/item/{itemId}")]
        public async Task<IActionResult> RemoveFromWishlist(int userId, int itemId)
        {
            var result = await _wishlistService.RemoveFromWishlistAsync(userId, itemId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item removed from wishlist successfully",
                Data = result
            });
        }
    }
}