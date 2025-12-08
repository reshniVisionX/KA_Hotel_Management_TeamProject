using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class ManagerRestaurantRepository : IRestaurantRepository
    {
        private readonly BookingContext _context;

        public ManagerRestaurantRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurants>> GetAllAsync()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<Restaurants?> GetByIdAsync(int id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == id);
        }

        public async Task<IEnumerable<Restaurants>> GetByManagerIdAsync(int managerId)
        {
            return await _context.Restaurants.Where(r => r.ManagerId == managerId).ToListAsync();
        }

        public async Task<Restaurants> AddAsync(Restaurants restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurants> UpdateAsync(Restaurants restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == id);
            if (existing == null) return false;

            _context.Restaurants.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Restaurants>> SearchAsync(string? name, string? location)
        {
            var query = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(r => r.RestaurantName.Contains(name));

            if (!string.IsNullOrEmpty(location))
                query = query.Where(r => r.Location.Contains(location) || r.City.Contains(location));

            return await query.ToListAsync();
        }

        public async Task<int?> GetRestaurantIdByManagerIdAsync(int managerId)
        {
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.ManagerId == managerId);
            return restaurant?.RestaurantId;
        }

        public async Task<RestaurantBookingSystem.Model.Manager.ManagerDetails?> GetManagerByUserIdAsync(int userId)
        {
            return await _context.ManagerDetails
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }
    }
}