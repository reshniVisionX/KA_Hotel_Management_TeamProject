using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class PaymentsRepository : IPaymentRepository
    {
        private readonly BookingContext _context;

        public PaymentsRepository(BookingContext context)
        {
            _context = context;
        }



        public async Task<IEnumerable<Payment>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Payment
                .Include(p => p.Order)
                .Where(p => p.Order!.RestaurantId == restaurantId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByRestaurantAndDateRangeAsync(int restaurantId, DateTime startDate, DateTime endDate)
        {
            return await _context.Payment
                .Include(p => p.Order)
                .Where(p => p.Order!.RestaurantId == restaurantId && 
                           p.PaymentDate >= startDate && 
                           p.PaymentDate <= endDate)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByRestaurantAndMethodAsync(int restaurantId, PayMethod method)
        {
            return await _context.Payment
                .Include(p => p.Order)
                .Where(p => p.Order!.RestaurantId == restaurantId && p.PayMethod == method)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetTodayPaymentsAsync(int restaurantId)
        {
            var today = DateTime.Today;
            return await _context.Payment
                .Include(p => p.Order)
                .Where(p => p.Order!.RestaurantId == restaurantId && p.PaymentDate.Date == today)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<object> GetPaymentSummaryAsync(int restaurantId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Payment
                .Include(p => p.Order)
                .Where(p => p.Order!.RestaurantId == restaurantId);

            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDate >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDate <= endDate.Value);

            var payments = await query.ToListAsync();

            return new
            {
                TotalPayments = payments.Count,
                TotalRevenue = payments.Sum(p => p.Amount),
                CompletedPayments = payments.Count(p => p.Status == PaymentStatus.Completed),
                PendingPayments = payments.Count(p => p.Status == PaymentStatus.Pending),
                CashPayments = payments.Count(p => p.PayMethod == PayMethod.Cash),
                UpiPayments = payments.Count(p => p.PayMethod == PayMethod.UPI),
                CashRevenue = payments.Where(p => p.PayMethod == PayMethod.Cash).Sum(p => p.Amount),
                UpiRevenue = payments.Where(p => p.PayMethod == PayMethod.UPI).Sum(p => p.Amount),
                AverageTransactionValue = payments.Any() ? payments.Average(p => p.Amount) : 0
            };
        }
    }
}