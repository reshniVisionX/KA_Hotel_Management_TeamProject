using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> CreateReviewAsync(Review review);
        Task<Review?> GetReviewByIdAsync(int reviewId);
        Task<List<Review>> GetRestaurantReviewsAsync(int restaurantId);
        Task<List<Review>> GetUserReviewsAsync(int userId);
        Task<Review> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<List<Review>> GetLatestReviewsAsync(int restaurantId, int count = 10);
        Task<List<Review>> GetTopRatedReviewsAsync(int restaurantId, int count = 10);
        Task<List<Review>> GetAllReviewsAsync();
        Task<bool> ValidateItemExistsAsync(int itemId, int restaurantId);
    }
}