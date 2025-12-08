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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsService _reviewService;

        public ReviewsController(IReviewsService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var result = await _reviewService.GetAllReviewsAsync();
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Reviews retrieved successfully",
                Data = result
            });
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetReviewsByRestaurant(int restaurantId)
        {
            if (restaurantId <= 0)
                throw new AppException("Restaurant ID must be greater than 0");

            var result = await _reviewService.GetReviewsByRestaurantAsync(restaurantId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Reviews retrieved successfully",
                Data = result
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReviewsByUserId(int userId)
        {
            if (userId <= 0)
                throw new AppException("User ID must be greater than 0");

            var result = await _reviewService.GetReviewsByUserIdAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Reviews retrieved successfully",
                Data = result
            });
        }

        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReview(int reviewId)
        {
            if (reviewId <= 0)
                throw new AppException("Review ID must be greater than 0");

            var result = await _reviewService.GetReviewAsync(reviewId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Review retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createDto)
        {
            if (!ModelState.IsValid)
                throw new AppException("Invalid data provided");

            var result = await _reviewService.CreateReviewAsync(createDto);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Review created successfully",
                Data = result
            });
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            if (reviewId <= 0)
                throw new AppException("Review ID must be greater than 0");

            var result = await _reviewService.DeleteReviewAsync(reviewId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Review deleted successfully",
                Data = result
            });
        }
    }
}