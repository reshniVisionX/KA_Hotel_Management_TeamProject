using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.Repository
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly BookingContext _context;

        public UserAddressRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<List<DeliveryAddress>> GetAddressesByUserIdAsync(int userId)
        {
            return await _context.DeliveryAddress
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}