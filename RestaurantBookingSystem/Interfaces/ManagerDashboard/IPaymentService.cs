using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Interface
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetPaymentsByRestaurantAsync(int restaurantId, DateTime? startDate, DateTime? endDate, string? paymentMethod);
        Task<IEnumerable<PaymentDto>> GetTodayPaymentsAsync(int restaurantId);
        Task<PaymentSummaryDto> GetPaymentSummaryAsync(int restaurantId, DateTime? startDate, DateTime? endDate);
    }
}