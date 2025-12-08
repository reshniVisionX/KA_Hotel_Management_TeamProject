using RestaurantBookingSystem.DTO;

namespace RestaurantBookingSystem.Interface.IService
{
    public interface IReviewsService
    {
        Task<List<ReviewDto>> GetAllReviewsAsync();
        Task<List<ReviewDto>> GetReviewsByRestaurantAsync(int restaurantId);
        Task<List<ReviewDto>> GetReviewsByUserIdAsync(int userId);
        Task<ReviewDto?> GetReviewAsync(int reviewId);
        Task<bool> CreateReviewAsync(CreateReviewDto createDto);
        Task<bool> DeleteReviewAsync(int reviewId);
    }
}