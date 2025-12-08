using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Delivery;
using System;

namespace RestaurantBookingSystem.Repository
{
    public class DeliveryAddressRepository : IDeliveryAddress
    {

        private readonly BookingContext _context;

        public DeliveryAddressRepository(BookingContext context)
        {
            _context = context;
        }
        public async Task<DeliveryAddress> AddAddressAsync(DeliveryAddress address)
        {
            await _context.DeliveryAddress.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            var entity = await _context.DeliveryAddress.FindAsync(addressId);
            if (entity == null) return false;

            _context.DeliveryAddress.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DeliveryAddress?> GetAddressByIdAsync(int addressId)
        {
            return await _context.DeliveryAddress.FindAsync(addressId);
        }

        public async Task<IEnumerable<DeliveryAddress>> GetAddressesByUserAsync(int userId)
        {
            return await _context.DeliveryAddress
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<DeliveryAddress?> GetDefaultAddressAsync(int userId)
        {
            return await _context.DeliveryAddress
                 .Where(x => x.UserId == userId && x.IsDefault)
                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DeliveryAddress>> GetUserAddresses(int userId)
        {
            return await _context.DeliveryAddress
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> SetDefaultAddressAsync(int userId, int addressId)
        {
             var addresses = await _context.DeliveryAddress
                .Where(x => x.UserId == userId)
                .ToListAsync();

            foreach (var addr in addresses)
                addr.IsDefault = false;

            var defaultAddress = addresses.FirstOrDefault(x => x.AddressId == addressId);

            if (defaultAddress == null) return false;

            defaultAddress.IsDefault = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DeliveryAddress> UpdateAddressAsync(DeliveryAddress address)
        {
            _context.DeliveryAddress.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }
}
