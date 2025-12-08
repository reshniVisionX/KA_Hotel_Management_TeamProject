using RestaurantBookingSystem.DTOorder;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;
namespace RestaurantBookingSystem.Services
{
    public class PaymentService
    {
        private readonly IPayment _repo;
        private readonly IOrders _ordrepo;
        public PaymentService(IPayment repo, IOrders ordrepo)
        {
            _repo = repo;
            _ordrepo = ordrepo;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<Payment?> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId)
            => await _repo.GetByOrderIdAsync(orderId);

        public async Task<Payment> CreateAsync(CreatePaymentDto dto)
        {
            // 1. Validate OrderId
            if (dto.OrderId <= 0)
                throw new AppException("Invalid OrderId");

            // 2. Fetch Order
            var order = await _ordrepo.GetOrderByIdAsync(dto.OrderId);
            if (order == null)
                throw new AppException("Order not found");

            // 3. Create Payment Row
            var payment = new Payment
            {
                OrderId = dto.OrderId,
                Amount = order.TotalAmount,
                PayMethod = dto.PayMethod,
                Status = PaymentStatus.Pending
            };

            var createdPayment = await _repo.CreateAsync(payment);

            // 4. Assign Delivery Person (Available Only)
            var deliveryPerson = await _repo
                .GetFirstAvailableAsync();

            if (deliveryPerson == null)
                throw new AppException("No delivery personnel available at the moment.");

            // Change his status → OnDelivery
            deliveryPerson.Status = DeliveryPersonStatus.OnDelivery;
            deliveryPerson.TotalDeliveries += 1;

            deliveryPerson.AverageRating = Math.Min(deliveryPerson.AverageRating + 0.5, 5.0);
            await _repo.UpdateAsync(deliveryPerson);

            // 5. Fetch default Delivery Address for this user
            var defaultAddress = await _repo.GetDefaultAddressAsync(order.UserId);

            if (defaultAddress == null)
                throw new AppException("User has no default delivery address");

            // 6. Create Delivery Entry
            var now = DateTime.Now;
            var delivery = new Delivery
            {
                OrderId = order.OrderId,
                DeliveryPersonId = deliveryPerson.DeliveryPersonId,
                DeliveryStatus = DeliveryStatus.Pending,
                EstimatedDeliveryTime = now.AddMinutes(10),
                DeliveredAt = now.AddMinutes(10),
                Instructions = "Thank you for your purchase! Your order will reach you shortly. Have a great day!",
                AddressId = defaultAddress.AddressId
            };

            var createdDelivery = await _repo.CreateAsync(delivery);

            return createdPayment;
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
