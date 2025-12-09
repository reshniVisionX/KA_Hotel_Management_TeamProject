using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class TableBookingRepository : ITableBookingRepository
    {
        private readonly BookingContext _context;

        public TableBookingRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Table)
                    .ThenInclude(t => t.Restaurants)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetBookingsByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Table)
                    .ThenInclude(t => t.Restaurants)
                .Where(r => r.Table.RestaurantId == restaurantId)
                .ToListAsync();
        }
    }
}