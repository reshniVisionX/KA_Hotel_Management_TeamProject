using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Services
{
    public class DeliveryService
    {
        private readonly IDelivery _repository;

        public DeliveryService(IDelivery repository)
        {
            _repository = repository;
        }

        public async Task<DeliveryResponseDto> CreateDeliveryAsync(DeliveryCreateDto dto)
        {
            // Validation
            if (dto.OrderId <= 0) throw new AppException("Invalid OrderId");
            if (dto.AddressId <= 0) throw new AppException("Invalid AddressId");

            var delivery = await _repository.CreateDeliveryAsync(dto.OrderId, dto.AddressId);
            
            // Model to DTO mapping
            return new DeliveryResponseDto
            {
                DeliveryId = delivery.DeliveryId,
                OrderId = delivery.OrderId,
                DeliveryStatus = delivery.DeliveryStatus,
                EstimatedDeliveryTime = delivery.EstimatedDeliveryTime,
                DeliveredAt = delivery.DeliveredAt,
                DeliveryPersonName = delivery.DeliveryPerson?.DeliveryName,
                DeliveryPersonMobile = delivery.DeliveryPerson?.MobileNo,
                Address = delivery.DeliveryAddress?.Address,
                City = delivery.DeliveryAddress?.City,
                Pincode = delivery.DeliveryAddress?.Pincode
            };
        }

        public async Task<DeliveryResponseDto> GetDeliveryByIdAsync(int id)
        {
            var delivery = await _repository.GetDeliveryByIdAsync(id);
            if (delivery == null)
                throw new AppException($"Delivery with ID {id} not found");

            // Model to DTO mapping
            return new DeliveryResponseDto
            {
                DeliveryId = delivery.DeliveryId,
                OrderId = delivery.OrderId,
                DeliveryStatus = delivery.DeliveryStatus,
                EstimatedDeliveryTime = delivery.EstimatedDeliveryTime,
                DeliveredAt = delivery.DeliveredAt,
                DeliveryPersonName = delivery.DeliveryPerson?.DeliveryName,
                DeliveryPersonMobile = delivery.DeliveryPerson?.MobileNo,
                Address = delivery.DeliveryAddress?.Address,
                City = delivery.DeliveryAddress?.City,
                Pincode = delivery.DeliveryAddress?.Pincode
            };
        }

        public async Task<IEnumerable<DeliveryResponseDto>> GetDeliveriesByUserAsync(int userId)
        {
            var deliveries = await _repository.GetDeliveriesByUserIdAsync(userId);
            
            // Model to DTO mapping
            return deliveries.Select(d => new DeliveryResponseDto
            {
                DeliveryId = d.DeliveryId,
                OrderId = d.OrderId,
                DeliveryStatus = d.DeliveryStatus,
                EstimatedDeliveryTime = d.EstimatedDeliveryTime,
                DeliveredAt = d.DeliveredAt,
                DeliveryPersonName = d.DeliveryPerson?.DeliveryName,
                DeliveryPersonMobile = d.DeliveryPerson?.MobileNo,
                Address = d.DeliveryAddress?.Address,
                City = d.DeliveryAddress?.City,
                Pincode = d.DeliveryAddress?.Pincode
            });
        }

        public async Task<DeliveryResponseDto> UpdateDeliveryStatusAsync(int deliveryId, DeliveryStatusUpdateDto dto)
        {
            var delivery = await _repository.UpdateDeliveryStatusAsync(deliveryId, dto.Status);
            if (delivery == null)
                throw new AppException($"Delivery with ID {deliveryId} not found");

            // Model to DTO mapping
            return new DeliveryResponseDto
            {
                DeliveryId = delivery.DeliveryId,
                OrderId = delivery.OrderId,
                DeliveryStatus = delivery.DeliveryStatus,
                EstimatedDeliveryTime = delivery.EstimatedDeliveryTime,
                DeliveredAt = delivery.DeliveredAt,
                DeliveryPersonName = delivery.DeliveryPerson?.DeliveryName,
                DeliveryPersonMobile = delivery.DeliveryPerson?.MobileNo,
                Address = delivery.DeliveryAddress?.Address,
                City = delivery.DeliveryAddress?.City,
                Pincode = delivery.DeliveryAddress?.Pincode
            };
        }

        public async Task<string> CompleteDeliveryAsync(DeliveryCompletionDto dto)
        {
            var delivery = await _repository.GetDeliveryByIdAsync(dto.DeliveryId);
            if (delivery == null)
                throw new AppException($"Delivery with ID {dto.DeliveryId} not found");

            if (delivery.DeliveryPerson?.otp != dto.Otp)
                throw new AppException("Invalid OTP");

            await _repository.UpdateDeliveryStatusAsync(dto.DeliveryId, DeliveryStatus.Delivered);
            return "Delivery completed successfully";
        }
    }
}