using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookingContext _context;

        public ReviewRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            await _context.Review.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }



        public async Task<Review?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task<List<Review>> GetRestaurantReviewsAsync(int restaurantId)
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<List<Review>> GetUserReviewsAsync(int userId)
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review> UpdateReviewAsync(Review review)
        {
            _context.Review.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Review.FindAsync(reviewId);
            if (review == null) return false;

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Review>> GetLatestReviewsAsync(int restaurantId, int count = 10)
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.ReviewDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Review>> GetTopRatedReviewsAsync(int restaurantId, int count = 10)
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.Rating)
                .ThenByDescending(r => r.ReviewDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _context.Review
                .Include(r => r.User)
                .Include(r => r.Restaurant)
                .ToListAsync();
        }

        public async Task<bool> ValidateItemExistsAsync(int itemId, int restaurantId)
        {
            return await _context.MenuList
                .AnyAsync(m => m.ItemId == itemId && m.RestaurantId == restaurantId);
        }
    }
}