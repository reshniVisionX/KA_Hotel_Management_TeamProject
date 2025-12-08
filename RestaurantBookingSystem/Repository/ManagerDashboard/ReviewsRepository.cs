using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Repository
{
    public class ManagerReviewsRepository : IReviewsRepository
    {
        private readonly BookingContext _context;

        public ManagerReviewsRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Review
                .Include(r => r.User)
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id, int restaurantId)
        {
            return await _context.Review
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReviewId == id && r.RestaurantId == restaurantId);
        }

        public async Task<IEnumerable<Review>> GetByRatingRangeAsync(int restaurantId, decimal minRating, decimal maxRating)
        {
            return await _context.Review
                .Include(r => r.User)
                .Where(r => r.RestaurantId == restaurantId && r.Rating >= minRating && r.Rating <= maxRating)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByDateRangeAsync(int restaurantId, DateTime startDate, DateTime endDate)
        {
            return await _context.Review
                .Include(r => r.User)
                .Where(r => r.RestaurantId == restaurantId && r.ReviewDate >= startDate && r.ReviewDate <= endDate)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<decimal> GetAverageRatingByRestaurantAsync(int restaurantId)
        {
            var reviews = await _context.Review
                .Where(r => r.RestaurantId == restaurantId)
                .ToListAsync();

            return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        }


    }
}