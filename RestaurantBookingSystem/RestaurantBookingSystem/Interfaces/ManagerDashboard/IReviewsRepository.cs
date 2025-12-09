using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Interface
{
    public interface IReviewsRepository
    {
        Task<IEnumerable<Review>> GetByRestaurantIdAsync(int restaurantId);
        Task<Review?> GetByIdAsync(int id, int restaurantId);
        Task<IEnumerable<Review>> GetByRatingRangeAsync(int restaurantId, decimal minRating, decimal maxRating);
        Task<IEnumerable<Review>> GetByDateRangeAsync(int restaurantId, DateTime startDate, DateTime endDate);
        Task<decimal> GetAverageRatingByRestaurantAsync(int restaurantId);

    }
}