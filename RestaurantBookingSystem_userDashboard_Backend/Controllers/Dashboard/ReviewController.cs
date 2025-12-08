using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IService;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    public class ReviewController : BaseController
    {
        private readonly IReviewsService _reviewService;

        public ReviewController(IReviewsService reviewService, ILogger<ReviewController> logger) : base(logger)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            return await ExecuteAsync(() => _reviewService.GetAllReviewsAsync(), "Reviews retrieved successfully");
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetReviewsByRestaurant(int restaurantId)
        {
            if (restaurantId <= 0)
            {
                _logger.LogWarning("Invalid restaurant ID: {RestaurantId}", restaurantId);
                return Ok(DTOs.ApiResponse<object>.ErrorResponse("Restaurant ID must be greater than 0"));
            }

            return await ExecuteAsync(() => _reviewService.GetReviewsByRestaurantAsync(restaurantId), "Reviews retrieved successfully");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReviewsByUserId(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                return Ok(DTOs.ApiResponse<object>.ErrorResponse("User ID must be greater than 0"));
            }

            return await ExecuteAsync(() => _reviewService.GetReviewsByUserIdAsync(userId), "Reviews retrieved successfully");
        }

        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReview(int reviewId)
        {
            if (reviewId <= 0)
            {
                _logger.LogWarning("Invalid review ID: {ReviewId}", reviewId);
                return Ok(DTOs.ApiResponse<object>.ErrorResponse("Review ID must be greater than 0"));
            }

            return await ExecuteAsync(() => _reviewService.GetReviewAsync(reviewId), "Review retrieved successfully");
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for review creation");
                return Ok(DTOs.ApiResponse.Error("Invalid data provided"));
            }

            return await ExecuteAsync(() => _reviewService.CreateReviewAsync(createDto), "Review created successfully", "Failed to create review");
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            if (reviewId <= 0)
            {
                _logger.LogWarning("Invalid review ID: {ReviewId}", reviewId);
                return Ok(DTOs.ApiResponse.Error("Review ID must be greater than 0"));
            }

            return await ExecuteAsync(() => _reviewService.DeleteReviewAsync(reviewId), "Review deleted successfully", "Review not found");
        }
    }
}