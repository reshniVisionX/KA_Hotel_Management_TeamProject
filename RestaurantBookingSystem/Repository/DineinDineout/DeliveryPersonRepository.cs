using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Delivery;
using System;

namespace RestaurantBookingSystem.Repository
{
    public class DeliveryPersonRepository : IDeliveryPerson
    {
        private readonly BookingContext _context;

        public DeliveryPersonRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<DeliveryPerson> CreateAsync(DeliveryPerson deliveryPerson)
        {
            _context.DeliveryPerson.Add(deliveryPerson);
            await _context.SaveChangesAsync();
            return deliveryPerson;
        }

        public async Task<IEnumerable<DeliveryPerson>> GetAllAsync()
        {
            return await _context.DeliveryPerson.ToListAsync();
        }

        public async Task<DeliveryPerson?> GetByIdAsync(int id)
        {
            return await _context.DeliveryPerson.FindAsync(id);
        }

        public async Task<DeliveryPerson?> UpdateAsync(DeliveryPerson deliveryPerson)
        {
            var existing = await _context.DeliveryPerson.FindAsync(deliveryPerson.DeliveryPersonId);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(deliveryPerson);
            await _context.SaveChangesAsync();
            return deliveryPerson;
        }

        public async Task<bool> UpdateStatusAsync(int id, DeliveryPersonStatus status)
        {
            var entity = await _context.DeliveryPerson.FindAsync(id);
            if (entity == null) return false;

            entity.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> UpdateOtpAsync(int id, int otp)
        {
            var entity = await _context.DeliveryPerson.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"DeliveryPerson with ID {id} not found");

            entity.otp = otp;
            await _context.SaveChangesAsync();
            return otp;
        }

        public async Task UpdateDeliveryStatsAsync(int deliveryPersonId)
        {
            var stats = await CalculateStatsAsync(deliveryPersonId);
            
            var deliveryPerson = await _context.DeliveryPerson.FindAsync(deliveryPersonId);
            if (deliveryPerson != null)
            {
                deliveryPerson.TotalDeliveries = stats.totalDeliveries;
                deliveryPerson.AverageRating = stats.averageRating;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(int totalDeliveries, double averageRating)> CalculateStatsAsync(int deliveryPersonId)
        {
            var deliveries = await _context.Delivery
                .Where(d => d.DeliveryPersonId == deliveryPersonId && d.DeliveryStatus == DeliveryStatus.Delivered)
                .ToListAsync();

            var totalDeliveries = deliveries.Count;
            
            // Get ratings from DeliveryPerson reviews/ratings (if implemented)
            // For now, return 0.0 as average rating since rating is moved to DeliveryPerson
            var averageRating = 0.0;

            return (totalDeliveries, averageRating);
        }
    }
}
