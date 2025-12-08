using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Repository
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly BookingContext _context;

        public UserPreferenceRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<List<UserPreferences>> GetAllUserPreferencesAsync()
        {
            return await _context.UserPreferences.ToListAsync();
        }

        public async Task<UserPreferences?> GetUserPreferenceAsync(int userId)
        {
            return await _context.UserPreferences.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserPreferences> CreateUserPreferenceAsync(UserPreferences preference)
        {
            _context.UserPreferences.Add(preference);
            await _context.SaveChangesAsync();
            return preference;
        }

        public async Task<bool> UpdateUserPreferenceAsync(UserPreferences preference)
        {
            _context.UserPreferences.Update(preference);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserPreferenceAsync(int userId)
        {
            var preference = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId);
            if (preference == null) return false;

            _context.UserPreferences.Remove(preference);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
