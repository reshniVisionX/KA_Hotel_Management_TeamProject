using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class TableRepository : ITableRepository
    {
        private readonly BookingContext _context;

        public TableRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<List<DineIn>> GetRestaurantTablesAsync(int restaurantId)
        {
            return await _context.DineIn
                .Include(t => t.Restaurants)
                .Where(t => t.RestaurantId == restaurantId)
                .OrderBy(t => t.TableNo)
                .ToListAsync();
        }

        public async Task<List<DineIn>> GetAvailableTablesAsync(int restaurantId)
        {
            var tables = await _context.DineIn
                .Include(t => t.Restaurants)
                .Where(t => t.RestaurantId == restaurantId && t.Status == TableStatus.Available)
                .OrderBy(t => t.TableNo)
                .ToListAsync();

            // Check if restaurant exists
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);
            
            return tables;
        }
    }
}