using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Interface
{
    public interface IReviewsService
    {
        Task<IEnumerable<ReviewsDto>> GetByRestaurantIdAsync(int restaurantId);
        Task<ReviewsDto?> GetByIdAsync(int id, int restaurantId);
        Task<IEnumerable<ReviewsDto>> GetByRatingRangeAsync(int restaurantId, decimal minRating, decimal maxRating);
        Task<IEnumerable<ReviewsDto>> GetByDateRangeAsync(int restaurantId, DateTime startDate, DateTime endDate);
        Task<decimal> GetAverageRatingByRestaurantAsync(int restaurantId);

    }
}