using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Manager;

namespace RestaurantBookingSystem.Repository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly BookingContext _context;

        public ManagerRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<ManagerDetails> CreateManagerAsync(ManagerDetails manager)
        {
            try
            {
                _context.ManagerDetails.Add(manager);
                await _context.SaveChangesAsync();
                return manager;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true ||
                    ex.InnerException?.Message.Contains("duplicate key") == true)
                {
                    throw new ArgumentException("Manager email already exists or user is already a manager");
                }
                if (ex.InnerException?.Message.Contains("FOREIGN KEY constraint failed") == true ||
                    ex.InnerException?.Message.Contains("foreign key constraint") == true)
                {
                    throw new ArgumentException("Invalid User ID");
                }
                throw;
            }
        }

        public async Task<ManagerDetails?> GetManagerByEmailAsync(string email)
        {
            return await _context.ManagerDetails.FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task<ManagerDetails?> GetManagerByUserIdAsync(int userId)
        {
            return await _context.ManagerDetails.FirstOrDefaultAsync(m => m.UserId == userId);
        }
    }
}
