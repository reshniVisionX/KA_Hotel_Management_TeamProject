using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IService;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    public class WishlistController : BaseController
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService, ILogger<WishlistController> logger) : base(logger)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserWishlist(int userId)
        {
            return await ExecuteAsync(() => _wishlistService.GetUserWishlistAsync(userId), "Wishlist retrieved successfully");
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] CreateWishlistDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for wishlist creation");
                return Ok(DTOs.ApiResponse.Error("Invalid data provided"));
            }

            return await ExecuteAsync(() => _wishlistService.AddToWishlistAsync(createDto), "Item added to wishlist successfully", "Failed to add item to wishlist");
        }

        [HttpDelete("{wishlistId}")]
        public async Task<IActionResult> RemoveFromWishlist(int wishlistId)
        {
            return await ExecuteAsync(() => _wishlistService.RemoveFromWishlistAsync(wishlistId), "Item removed from wishlist successfully", "Wishlist item not found");
        }

        [HttpDelete("user/{userId}/item/{itemId}")]
        public async Task<IActionResult> RemoveFromWishlist(int userId, int itemId)
        {
            return await ExecuteAsync(() => _wishlistService.RemoveFromWishlistAsync(userId, itemId), "Item removed from wishlist successfully", "Wishlist item not found");
        }
    }
}