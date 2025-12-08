using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class PaymentRepository : IPayment
    {
        private readonly BookingContext _context;

        public PaymentRepository(BookingContext context)
        {
            _context = context;
        }
        public async Task<Payment> CreateAsync(Payment payment)
        {
            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payment
               .Include(p => p.Order)
               .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payment
               .Include(p => p.Order)
               .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payment
                 .Where(p => p.OrderId == orderId)
                 .Include(p => p.Order)
                 .ToListAsync();
        }

        public async Task<Payment?> UpdateAsync(Payment payment)
        {
            var existing = await _context.Payment.FindAsync(payment.PaymentId);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(payment);
            await _context.SaveChangesAsync();
            return existing;
        }
        // ---  Delivery ---

        public async Task<DeliveryPerson?> GetFirstAvailableAsync()
        {
            return await _context.DeliveryPerson
                .Where(dp => dp.Status == DeliveryPersonStatus.Available)
                .OrderBy(dp => dp.TotalDeliveries)
                .FirstOrDefaultAsync();
        }
        public async Task<DeliveryAddress?> GetDefaultAddressAsync(int userId)
        {
            return await _context.DeliveryAddress
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault == true);
        }
        public async Task<Delivery> CreateAsync(Delivery delivery)
        {
            _context.Delivery.Add(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }
        public async Task UpdateAsync(DeliveryPerson person)
        {
            _context.DeliveryPerson.Update(person);
            await _context.SaveChangesAsync();
        }

    }
}
