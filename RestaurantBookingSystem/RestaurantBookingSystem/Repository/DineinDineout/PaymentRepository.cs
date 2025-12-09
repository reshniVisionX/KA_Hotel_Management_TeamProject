using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
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
    }
}
