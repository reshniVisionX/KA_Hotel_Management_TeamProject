using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces.IRepository;
using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Model.Manager;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly BookingContext _context;

        public AdminRepository(BookingContext context)
        {
            _context = context;
        }

        // ------------------- Restaurants -------------------
        public async Task<IEnumerable<Restaurants>> GetAllRestaurantsAsync()
        {
            return await _context.Restaurants
                         .Include(r => r.Manager)                 // ManagerDetails
                         .ToListAsync();
        }

        public async Task<IEnumerable<Restaurants>> FilterRestaurants(
            int? id,
            string? city,
            RestaurantCategory? category,
            FoodType? type,
            string? managerName)
        {
            var query = _context.Restaurants
                        .Include(r => r.Manager)
                        .AsQueryable();

            if (id.HasValue)
                query = query.Where(r => r.RestaurantId == id.Value);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(r => EF.Functions.Like(r.City, $"%{city}%"));

            if (category.HasValue)
                query = query.Where(r => r.RestaurantCategory == category.Value);

            if (type.HasValue)
                query = query.Where(r => r.RestaurantType == type.Value);

            if (!string.IsNullOrWhiteSpace(managerName))
                query = query.Where(r => r.Manager != null &&
                                         EF.Functions.Like(r.Manager.ManagerName, $"%{managerName}%"));

            return await query.ToListAsync();
        }

        public async Task<Restaurants?> GetRestaurantByManagerIdAsync(int managerId)
        {
            return await _context.Restaurants
                         .Include(r => r.Manager)
                         .FirstOrDefaultAsync(r => r.ManagerId == managerId);
        }

        public async Task<bool> ToggleRestaurantStatus(int restaurantId)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if (restaurant == null) return false;

            restaurant.IsActive = !restaurant.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        // ------------------- Managers -------------------
        // NOTE: managers are stored in ManagerDetails (not Users).
        public async Task<IEnumerable<Users>> GetAllManagersAsync(int roleId)
        {
            // If you truly want the Users who are managers (joined by RoleId),
            // return Users filtered by RoleId. Otherwise, you can return ManagerDetails.
            // Here I return Users where RoleId == roleId (keeps the original signature).
            return await _context.Users
                        .Where(u => u.RoleId == roleId)
                        .ToListAsync();
        }

        public async Task<bool> ToggleManagerStatus(int managerId)
        {
            // Toggle the ManagerDetails.IsActive flag (managerId is ManagerDetails.ManagerId)
            var manager = await _context.ManagerDetails.FindAsync(managerId);
            if (manager == null) return false;

            manager.IsActive = !manager.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        // ------------------- Analytics -------------------
      
        public async Task<IEnumerable<Orders>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservation.ToListAsync();
        }


        public async Task<IEnumerable<Restaurants>> GetAllRestaurantsForAnalyticsAsync()
        {
            return await _context.Restaurants.Include(r => r.Manager).ToListAsync();
        }

        public async Task<IEnumerable<ManagerDetails>> GetAllManagerDetailsAsync()
        {
            return await _context.ManagerDetails.ToListAsync();
        }
    }
}
