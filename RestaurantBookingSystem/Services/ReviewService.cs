using AutoMapper;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReviewResponseDto> CreateReviewAsync(int userId, ReviewCreateDto request)
        {
            var review = new Review
            {
                UserId = userId,
                RestaurantId = request.RestaurantId,
                Rating = request.Rating,
                Comments = request.Comments,
                ReviewDate = DateTime.Now
            };

            var createdReview = await _repository.CreateReviewAsync(review);
            var reviewWithDetails = await _repository.GetReviewByIdAsync(createdReview.ReviewId);
            return _mapper.Map<ReviewResponseDto>(reviewWithDetails);
        }



        public async Task<List<ReviewResponseDto>> GetRestaurantReviewsAsync(int restaurantId)
        {
            var reviews = await _repository.GetRestaurantReviewsAsync(restaurantId);
            return _mapper.Map<List<ReviewResponseDto>>(reviews);
        }

        public async Task<List<ReviewResponseDto>> GetUserReviewsAsync(int userId)
        {
            var reviews = await _repository.GetUserReviewsAsync(userId);
            return _mapper.Map<List<ReviewResponseDto>>(reviews);
        }

        public async Task<ReviewResponseDto?> UpdateReviewAsync(int reviewId, ReviewUpdateDto request)
        {
            var existingReview = await _repository.GetReviewByIdAsync(reviewId);
            if (existingReview == null) return null;
            
            existingReview.Rating = request.Rating;
            existingReview.Comments = request.Comments;
            existingReview.ReviewDate = DateTime.Now;

            var updatedReview = await _repository.UpdateReviewAsync(existingReview);
            return _mapper.Map<ReviewResponseDto>(updatedReview);
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            return await _repository.DeleteReviewAsync(reviewId);
        }

        public async Task<ReviewRatingDto> GetAverageRatingAsync(int restaurantId)
        {
            var reviews = await _repository.GetRestaurantReviewsAsync(restaurantId);
            
            return new ReviewRatingDto
            {
                RestaurantId = restaurantId,
                RestaurantName = reviews.FirstOrDefault()?.Restaurant?.RestaurantName ?? "",
                AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0,
                TotalReviews = reviews.Count,
                FiveStarCount = reviews.Count(r => r.Rating >= 9),
                FourStarCount = reviews.Count(r => r.Rating >= 7 && r.Rating < 9),
                ThreeStarCount = reviews.Count(r => r.Rating >= 5 && r.Rating < 7),
                TwoStarCount = reviews.Count(r => r.Rating >= 3 && r.Rating < 5),
                OneStarCount = reviews.Count(r => r.Rating < 3)
            };
        }

        public async Task<List<ReviewResponseDto>> GetLatestReviewsAsync(int restaurantId)
        {
            var reviews = await _repository.GetLatestReviewsAsync(restaurantId);
            return _mapper.Map<List<ReviewResponseDto>>(reviews);
        }

        public async Task<List<ReviewResponseDto>> GetTopRatedReviewsAsync(int restaurantId)
        {
            var reviews = await _repository.GetTopRatedReviewsAsync(restaurantId);
            return _mapper.Map<List<ReviewResponseDto>>(reviews);
        }

        public async Task<ReviewResponseDto> CreateItemReviewAsync(int userId, ItemReviewCreateDto request)
        {
            // Validate item exists in restaurant
            var itemExists = await ValidateItemInRestaurantAsync(request.ItemId, request.RestaurantId);
            if (!itemExists)
            {
                throw new InvalidOperationException("Item does not exist in this restaurant");
            }

            var review = new Review
            {
                UserId = userId,
                RestaurantId = request.RestaurantId,
                Rating = request.Rating,
                Comments = $"ITEM:{request.ItemId}|{request.Comments}",
                ReviewDate = DateTime.Now
            };

            var createdReview = await _repository.CreateReviewAsync(review);
            var reviewWithDetails = await _repository.GetReviewByIdAsync(createdReview.ReviewId);
            return _mapper.Map<ReviewResponseDto>(reviewWithDetails);
        }

        public async Task<List<ReviewResponseDto>> GetItemReviewsAsync(int itemId)
        {
            var allReviews = await _repository.GetAllReviewsAsync();
            var itemReviews = allReviews.Where(r => r.Comments != null && r.Comments.StartsWith($"ITEM:{itemId}|")).ToList();
            return _mapper.Map<List<ReviewResponseDto>>(itemReviews);
        }

        public async Task<ItemRatingDto> GetItemRatingAsync(int itemId)
        {
            var allReviews = await _repository.GetAllReviewsAsync();
            var itemReviews = allReviews.Where(r => r.Comments != null && r.Comments.StartsWith($"ITEM:{itemId}|")).ToList();
            
            return new ItemRatingDto
            {
                ItemId = itemId,
                ItemName = "",
                AverageRating = itemReviews.Any() ? itemReviews.Average(r => r.Rating) : 0,
                TotalReviews = itemReviews.Count
            };
        }

        private async Task<bool> ValidateItemInRestaurantAsync(int itemId, int restaurantId)
        {
            return await _repository.ValidateItemExistsAsync(itemId, restaurantId);
        }


    }
}