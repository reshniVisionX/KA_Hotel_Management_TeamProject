using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Services
{
    public class ManagerReviewsService : IReviewsService
    {
        private readonly IReviewsRepository _repo;

        public ManagerReviewsService(IReviewsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ReviewsDto>> GetByRestaurantIdAsync(int restaurantId)
        {
            var reviews = await _repo.GetByRestaurantIdAsync(restaurantId);
            return reviews.Select(MapToDto);
        }

        public async Task<ReviewsDto?> GetByIdAsync(int id, int restaurantId)
        {
            var review = await _repo.GetByIdAsync(id, restaurantId);
            return review == null ? null : MapToDto(review);
        }

        public async Task<IEnumerable<ReviewsDto>> GetByRatingRangeAsync(int restaurantId, decimal minRating, decimal maxRating)
        {
            var reviews = await _repo.GetByRatingRangeAsync(restaurantId, minRating, maxRating);
            return reviews.Select(MapToDto);
        }

        public async Task<IEnumerable<ReviewsDto>> GetByDateRangeAsync(int restaurantId, DateTime startDate, DateTime endDate)
        {
            var reviews = await _repo.GetByDateRangeAsync(restaurantId, startDate, endDate);
            return reviews.Select(MapToDto);
        }

        public async Task<decimal> GetAverageRatingByRestaurantAsync(int restaurantId)
        {
            return await _repo.GetAverageRatingByRestaurantAsync(restaurantId);
        }

        private static ReviewsDto MapToDto(Review review)
        {
            return new ReviewsDto
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId,

                RestaurantId = review.RestaurantId,
                Rating = review.Rating,
                Comments = review.Comments,
                ReviewDate = review.ReviewDate
            };
        }
    }
}