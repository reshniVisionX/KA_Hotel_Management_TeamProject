using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        [HttpPost("create/{userId}")]
        public async Task<ActionResult> CreateReview(int userId, [FromBody] ReviewCreateDto request)
        {
            var result = await _service.CreateReviewAsync(userId, request);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Review created successfully",
                Data = result
            });
        }

        [HttpGet("restaurant/{id}")]
        public async Task<ActionResult> GetRestaurantReviews(int id)
        {
            var result = await _service.GetRestaurantReviewsAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Restaurant reviews retrieved successfully",
                Data = result
            });
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult> GetUserReviews(int id)
        {
            var result = await _service.GetUserReviewsAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "User reviews retrieved successfully",
                Data = result
            });
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateReview(int id, [FromBody] ReviewUpdateDto request)
        {
            var result = await _service.UpdateReviewAsync(id, request);
            if (result == null) throw new AppException("Review not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Review updated successfully",
                Data = result
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var result = await _service.DeleteReviewAsync(id);
            if (!result) throw new AppException("Review not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Review deleted successfully",
                Data = null
            });
        }

        [HttpGet("rating/{restaurantId}")]
        public async Task<ActionResult> GetAverageRating(int restaurantId)
        {
            var result = await _service.GetAverageRatingAsync(restaurantId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Average rating retrieved successfully",
                Data = result
            });
        }

        [HttpGet("restaurant/{id}/latest")]
        public async Task<ActionResult> GetLatestReviews(int id)
        {
            var result = await _service.GetLatestReviewsAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Latest reviews retrieved successfully",
                Data = result
            });
        }

        [HttpGet("restaurant/{id}/top-rated")]
        public async Task<ActionResult> GetTopRatedReviews(int id)
        {
            var result = await _service.GetTopRatedReviewsAsync(id);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Top rated reviews retrieved successfully",
                Data = result
            });
        }

        [HttpPost("item/{userId}")]
        public async Task<ActionResult> CreateItemReview(int userId, [FromBody] ItemReviewCreateDto request)
        {
            var result = await _service.CreateItemReviewAsync(userId, request);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item review created successfully",
                Data = result
            });
        }

        [HttpGet("item/{itemId}")]
        public async Task<ActionResult> GetItemReviews(int itemId)
        {
            var result = await _service.GetItemReviewsAsync(itemId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item reviews retrieved successfully",
                Data = result
            });
        }

        [HttpGet("item/{itemId}/rating")]
        public async Task<ActionResult> GetItemRating(int itemId)
        {
            var result = await _service.GetItemRatingAsync(itemId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Item rating retrieved successfully",
                Data = result
            });
        }
    }
}