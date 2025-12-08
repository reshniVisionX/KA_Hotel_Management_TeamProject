using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Services
{
    public class PaymentsService : IPaymentService
    {
        private readonly IPaymentRepository _repo;

        public PaymentsService(IPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByRestaurantAsync(int restaurantId, DateTime? startDate, DateTime? endDate, string? paymentMethod)
        {
            IEnumerable<Payment> payments;
            
            if (startDate.HasValue && endDate.HasValue)
            {
                payments = await _repo.GetByRestaurantAndDateRangeAsync(restaurantId, startDate.Value, endDate.Value);
            }
            else if (!string.IsNullOrEmpty(paymentMethod) && Enum.TryParse<PayMethod>(paymentMethod, true, out var method))
            {
                payments = await _repo.GetByRestaurantAndMethodAsync(restaurantId, method);
            }
            else
            {
                payments = await _repo.GetByRestaurantIdAsync(restaurantId);
            }

            return payments.Select(MapToDto);
        }

        public async Task<IEnumerable<PaymentDto>> GetTodayPaymentsAsync(int restaurantId)
        {
            var payments = await _repo.GetTodayPaymentsAsync(restaurantId);
            return payments.Select(MapToDto);
        }

        public async Task<PaymentSummaryDto> GetPaymentSummaryAsync(int restaurantId, DateTime? startDate, DateTime? endDate)
        {
            var summary = await _repo.GetPaymentSummaryAsync(restaurantId, startDate, endDate);
            return new PaymentSummaryDto
            {
                TotalPayments = (int)((dynamic)summary).TotalPayments,
                TodayPayments = (int)((dynamic)summary).TodayPayments,
                CompletedPayments = (int)((dynamic)summary).CompletedPayments,
                PendingPayments = (int)((dynamic)summary).PendingPayments,
                TotalAmount = (decimal)((dynamic)summary).TotalAmount,
                TodayAmount = (decimal)((dynamic)summary).TodayAmount,
                CashPayments = (int)((dynamic)summary).CashPayments,
                UpiPayments = (int)((dynamic)summary).UpiPayments
            };
        }

        private static PaymentDto MapToDto(Payment p)
        {
            return new PaymentDto
            {
                PaymentId = p.PaymentId,
                OrderId = p.OrderId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PayMethod = p.PayMethod,
                Status = p.Status
            };
        }
    }
}