using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Repository
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly BookingContext _context;

        public UserProfileRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users?> GetUserAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<Users> CreateUserAsync(Users user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true ||
                    ex.InnerException?.Message.Contains("duplicate key") == true)
                {
                    throw new ArgumentException("Email already exists");
                }
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(Users user)
        {
            try
            {
                _context.Users.Update(user);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true ||
                    ex.InnerException?.Message.Contains("duplicate key") == true)
                {
                    throw new ArgumentException("Email already exists");
                }
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
