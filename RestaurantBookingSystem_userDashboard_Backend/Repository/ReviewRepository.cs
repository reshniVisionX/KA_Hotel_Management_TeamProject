using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Repository
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly BookingContext _context;

        public ReviewsRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByRestaurantAsync(int restaurantId)
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .Where(r => r.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewAsync(int reviewId)
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            try
            {
                _context.Review.Add(review);
                await _context.SaveChangesAsync();
                return review;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FOREIGN KEY constraint failed") == true ||
                    ex.InnerException?.Message.Contains("foreign key constraint") == true)
                {
                    throw new ArgumentException("Invalid User ID or Restaurant ID");
                }
                throw;
            }
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Review.FindAsync(reviewId);
            if (review == null) return false;

            _context.Review.Remove(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Review?> GetReviewByUserAndRestaurantAsync(int userId, int restaurantId)
        {
            return await _context.Review
                .FirstOrDefaultAsync(r => r.UserId == userId && r.RestaurantId == restaurantId);
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.UserId == userId);
        }

        public async Task<bool> RestaurantExistsAsync(int restaurantId)
        {
            return await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
        }
    }
}