using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces.Admin;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Model.Restaurant;
using System;

namespace RestaurantBookingSystem.Repository.Admin
{
    public class AdminDeliveryRepository : IDeliveryRespository
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
    
      public async Task<IEnumerable<MenuList>> GetAllMenuListAsync()
        {
            return await _context.MenuList

                .ToListAsync();
        }
        public async Task<Delivery?> GetDeliveryWithRelationsAsync(int deliveryId)
        {
            return await _context.Delivery
                .Include(d => d.DeliveryPerson)
                .Include(d => d.Order)
                .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
        }

        public async Task UpdateDeliveryAsync(Delivery delivery)
        {
            _context.Delivery.Update(delivery);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDeliveryPersonAsync(DeliveryPerson person)
        {
            _context.DeliveryPerson.Update(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Orders order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Delivery>> GetDeliveriesByPersonAsync(int deliveryPersonId)
        {
            return await _context.Delivery
                .Include(d => d.DeliveryPerson)
                .Include(d => d.DeliveryAddress)
                    .ThenInclude(a => a.User)   
                .Where(d => d.DeliveryPersonId == deliveryPersonId)
                .ToListAsync();
        }

    }
}
