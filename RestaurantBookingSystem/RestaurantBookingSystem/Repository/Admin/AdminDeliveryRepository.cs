using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Model.Delivery;
using System;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Interfaces.Admin;

namespace RestaurantBookingSystem.Repository.Admin
{
    public class AdminDeliveryRepository: IDeliveryRespository
    {
        private readonly BookingContext _context;

        public AdminDeliveryRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeliveryAddress>> GetUserAddressesAsync(int userId)
        {
            return await _context.DeliveryAddress
                                 .Where(x => x.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<DeliveryAddress?> GetAddressByIdAsync(int addressId)
        {
            return await _context.DeliveryAddress
                                 .FirstOrDefaultAsync(x => x.AddressId == addressId);
        }

        public async Task<DeliveryAddress> AddAsync(DeliveryAddress address)
        {
            _context.DeliveryAddress.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
