using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Interface.IRepository
{
    public interface IReviewsRepository
    {
        Task<List<Review>> GetAllReviewsAsync();
        Task<List<Review>> GetReviewsByRestaurantAsync(int restaurantId);
        Task<List<Review>> GetReviewsByUserIdAsync(int userId);
        Task<Review?> GetReviewAsync(int reviewId);
        Task<Review?> GetReviewByUserAndRestaurantAsync(int userId, int restaurantId);
        Task<Review> CreateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<bool> UserExistsAsync(int userId);
        Task<bool> RestaurantExistsAsync(int restaurantId);
    }
}