using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;

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
        public async Task<ActionResult<ReviewResponseDto>> CreateReview(int userId, [FromBody] ReviewCreateDto request)
        {
            var result = await _service.CreateReviewAsync(userId, request);
            return Ok(result);
        }



        [HttpGet("restaurant/{id}")]
        public async Task<ActionResult<List<ReviewResponseDto>>> GetRestaurantReviews(int id)
        {
            var result = await _service.GetRestaurantReviewsAsync(id);
            return Ok(result);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<ReviewResponseDto>>> GetUserReviews(int id)
        {
            var result = await _service.GetUserReviewsAsync(id);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<ReviewResponseDto>> UpdateReview(int id, [FromBody] ReviewUpdateDto request)
        {
            var result = await _service.UpdateReviewAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var result = await _service.DeleteReviewAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("rating/{restaurantId}")]
        public async Task<ActionResult<ReviewRatingDto>> GetAverageRating(int restaurantId)
        {
            var result = await _service.GetAverageRatingAsync(restaurantId);
            return Ok(result);
        }

        [HttpGet("restaurant/{id}/latest")]
        public async Task<ActionResult<List<ReviewResponseDto>>> GetLatestReviews(int id)
        {
            var result = await _service.GetLatestReviewsAsync(id);
            return Ok(result);
        }

        [HttpGet("restaurant/{id}/top-rated")]
        public async Task<ActionResult<List<ReviewResponseDto>>> GetTopRatedReviews(int id)
        {
            var result = await _service.GetTopRatedReviewsAsync(id);
            return Ok(result);
        }

        [HttpPost("item/{userId}")]
        public async Task<ActionResult<ReviewResponseDto>> CreateItemReview(int userId, [FromBody] ItemReviewCreateDto request)
        {
            var result = await _service.CreateItemReviewAsync(userId, request);
            return Ok(result);
        }

        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<List<ReviewResponseDto>>> GetItemReviews(int itemId)
        {
            var result = await _service.GetItemReviewsAsync(itemId);
            return Ok(result);
        }

        [HttpGet("item/{itemId}/rating")]
        public async Task<ActionResult<ItemRatingDto>> GetItemRating(int itemId)
        {
            var result = await _service.GetItemRatingAsync(itemId);
            return Ok(result);
        }
    }
}