using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Services
{
    public class DashboardReviewsService : IReviewsService
    {
        private readonly IReviewsRepository _reviewRepo;

        public DashboardReviewsService(IReviewsRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public async Task<List<ReviewsDto>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepo.GetAllReviewsAsync();
            return reviews.Select(r => new ReviewsDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                RestaurantId = r.RestaurantId,
                Rating = r.Rating,
                Comments = r.Comments,
                ReviewDate = r.ReviewDate,
                UserName = r.User != null ? $"{r.User.FirstName} {r.User.LastName}" : null,
                RestaurantName = r.Restaurant != null ? r.Restaurant.RestaurantName : null
            }).ToList();
        }

        public async Task<List<ReviewsDto>> GetReviewsByRestaurantAsync(int restaurantId)
        {
            if (restaurantId <= 0)
                throw new AppException("Restaurant ID must be greater than 0");

            var reviews = await _reviewRepo.GetReviewsByRestaurantAsync(restaurantId);
            return reviews.Select(r => new ReviewsDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                RestaurantId = r.RestaurantId,
                Rating = r.Rating,
                Comments = r.Comments,
                ReviewDate = r.ReviewDate,
                UserName = r.User != null ? $"{r.User.FirstName} {r.User.LastName}" : null,
                RestaurantName = r.Restaurant != null ? r.Restaurant.RestaurantName : null
            }).ToList();
        }

        public async Task<List<ReviewsDto>> GetReviewsByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new AppException("User ID must be greater than 0");

            var reviews = await _reviewRepo.GetReviewsByUserIdAsync(userId);
            return reviews.Select(r => new ReviewsDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                RestaurantId = r.RestaurantId,
                Rating = r.Rating,
                Comments = r.Comments,
                ReviewDate = r.ReviewDate,
                UserName = r.User != null ? $"{r.User.FirstName} {r.User.LastName}" : null,
                RestaurantName = r.Restaurant != null ? r.Restaurant.RestaurantName : null
            }).ToList();
        }

        public async Task<ReviewsDto?> GetReviewAsync(int reviewId)
        {
            if (reviewId <= 0)
                throw new AppException("Review ID must be greater than 0");

            var review = await _reviewRepo.GetReviewAsync(reviewId);
            if (review == null) return null;

            return new ReviewsDto
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId,
                RestaurantId = review.RestaurantId,
                Rating = review.Rating,
                Comments = review.Comments,
                ReviewDate = review.ReviewDate,
                UserName = review.User != null ? $"{review.User.FirstName} {review.User.LastName}" : null,
                RestaurantName = review.Restaurant != null ? review.Restaurant.RestaurantName : null
            };
        }

        public async Task<bool> CreateReviewAsync(CreateReviewDto createDto)
        {
            if (createDto.UserId <= 0)
                throw new AppException("User ID must be greater than 0");

            if (createDto.RestaurantId <= 0)
                throw new AppException("Restaurant ID must be greater than 0");

            if (createDto.Rating < 0 || createDto.Rating > 10)
                throw new AppException("Rating must be between 0 and 10");

            if (string.IsNullOrWhiteSpace(createDto.Comments))
                throw new AppException("Comments are required");

            // Check if user exists
            var userExists = await _reviewRepo.UserExistsAsync(createDto.UserId);
            if (!userExists)
                throw new AppException("User does not exist");

            // Check if restaurant exists
            var restaurantExists = await _reviewRepo.RestaurantExistsAsync(createDto.RestaurantId);
            if (!restaurantExists)
                throw new AppException("Restaurant does not exist");

            // Check for duplicate review (one review per user per restaurant)
            var existingReview = await _reviewRepo.GetReviewByUserAndRestaurantAsync(createDto.UserId, createDto.RestaurantId);
            if (existingReview != null)
                throw new AppException("User has already reviewed this restaurant");

            var review = new Review
            {
                UserId = createDto.UserId,
                RestaurantId = createDto.RestaurantId,
                Rating = createDto.Rating,
                Comments = createDto.Comments,
                ReviewDate = DateTime.Now
            };

            await _reviewRepo.CreateReviewAsync(review);
            return true;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            if (reviewId <= 0)
                throw new AppException("Review ID must be greater than 0");

            return await _reviewRepo.DeleteReviewAsync(reviewId);
        }
    }
}