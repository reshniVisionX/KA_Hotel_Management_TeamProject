using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Delivery;
using System;

namespace RestaurantBookingSystem.Repository
{
    public class DeliveryRepository : IDelivery
    {
        private readonly BookingContext _context;

        public DeliveryRepository(BookingContext context)
        {
            _context = context;
        }
        public async Task<Delivery> CreateDeliveryAsync(int orderId, int? addressId)
        {
            var deliveryPerson = await _context.DeliveryPerson
                 .Where(x => x.Status == DeliveryPersonStatus.Available)
                 .FirstOrDefaultAsync();

            if (deliveryPerson == null)
                throw new Exception("No delivery person available");

            deliveryPerson.Status = DeliveryPersonStatus.OnDelivery;

            var delivery = new Delivery
            {
                OrderId = orderId,
                DeliveryPersonId = deliveryPerson.DeliveryPersonId,
                AddressId = addressId,
                DeliveryStatus = DeliveryStatus.Pending
            };

            _context.Delivery.Add(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByUserIdAsync(int userId)
        {
            return await _context.Delivery
               .Include(x => x.Order)
               .Include(x => x.DeliveryPerson)
               .Include(x => x.DeliveryAddress)
               .Where(x => x.Order.UserId == userId)
               .ToListAsync();
        }

        public async Task<Delivery?> GetDeliveryByIdAsync(int deliveryId)
        {
            return await _context.Delivery
               .Include(x => x.DeliveryPerson)
               .Include(x => x.DeliveryAddress)
               .FirstOrDefaultAsync(x => x.DeliveryId == deliveryId);
        }

        public async Task<Delivery?> UpdateDeliveryStatusAsync(int deliveryId, DeliveryStatus status)
        {
            var delivery = await _context.Delivery
                 .Include(x => x.DeliveryPerson)
                 .Include(x => x.DeliveryAddress)
                 .FirstOrDefaultAsync(x => x.DeliveryId == deliveryId);

            if (delivery == null) return null;

            delivery.DeliveryStatus = status;

            if (status == DeliveryStatus.Delivered || status == DeliveryStatus.Failed)
            {
                delivery.DeliveredAt = DateTime.Now;
                if (delivery.DeliveryPerson != null)
                {
                    delivery.DeliveryPerson.Status = DeliveryPersonStatus.Available;
                    delivery.DeliveryPerson.TotalDeliveries++;
                }
            }

            await _context.SaveChangesAsync();
            return delivery;
        }
    }
}
