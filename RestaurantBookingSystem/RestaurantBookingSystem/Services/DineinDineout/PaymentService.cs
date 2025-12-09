using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;
namespace RestaurantBookingSystem.Services
{
    public class PaymentService
    {
        private readonly IPayment _repo;

        public PaymentService(IPayment repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<Payment?> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId)
            => await _repo.GetByOrderIdAsync(orderId);

        public async Task<Payment> CreateAsync(CreatePaymentDto dto)
        {
            // Validation
            if (dto.OrderId <= 0) throw new AppException("Invalid OrderId");
            if (dto.Amount <= 0) throw new AppException("Amount must be greater than 0");

            // DTO to Model mapping
            var payment = new Payment
            {
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                PayMethod = dto.PayMethod,
                Status = PaymentStatus.Pending
            };

            return await _repo.CreateAsync(payment);
        }

        public async Task<Payment> UpdateAsync(int paymentId, UpdatePaymentDto dto)
        {
            var existing = await _repo.GetByIdAsync(paymentId);
            if (existing == null)
                throw new AppException($"Payment with ID {paymentId} not found");

            // DTO to Model mapping
            existing.Status = dto.Status;

            return await _repo.UpdateAsync(existing);
        }
    }
}
