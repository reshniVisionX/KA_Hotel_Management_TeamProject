using RestaurantBookingSystem.DTOs;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateReviewAsync(int userId, ReviewCreateDto request);
        Task<List<ReviewResponseDto>> GetRestaurantReviewsAsync(int restaurantId);
        Task<List<ReviewResponseDto>> GetUserReviewsAsync(int userId);
        Task<ReviewResponseDto?> UpdateReviewAsync(int reviewId, ReviewUpdateDto request);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<ReviewRatingDto> GetAverageRatingAsync(int restaurantId);
        Task<List<ReviewResponseDto>> GetLatestReviewsAsync(int restaurantId);
        Task<List<ReviewResponseDto>> GetTopRatedReviewsAsync(int restaurantId);
        Task<ReviewResponseDto> CreateItemReviewAsync(int userId, ItemReviewCreateDto request);
        Task<List<ReviewResponseDto>> GetItemReviewsAsync(int itemId);
        Task<ItemRatingDto> GetItemRatingAsync(int itemId);
    }
}