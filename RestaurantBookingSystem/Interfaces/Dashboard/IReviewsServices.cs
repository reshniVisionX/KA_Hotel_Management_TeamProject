using RestaurantBookingSystem.DTO;

namespace RestaurantBookingSystem.Interface.IService
{
    public interface IReviewsService
    {
        Task<List<ReviewsDto>> GetAllReviewsAsync();
        Task<List<ReviewsDto>> GetReviewsByRestaurantAsync(int restaurantId);
        Task<List<ReviewsDto>> GetReviewsByUserIdAsync(int userId);
        Task<ReviewsDto?> GetReviewAsync(int reviewId);
        Task<bool> CreateReviewAsync(CreateReviewDto createDto);
        Task<bool> DeleteReviewAsync(int reviewId);
    }
}