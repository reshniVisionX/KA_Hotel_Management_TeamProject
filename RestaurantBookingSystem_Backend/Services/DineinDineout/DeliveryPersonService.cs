using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Services
{
    public class DeliveryPersonService
    {
        private readonly IDeliveryPerson _repository;

        public DeliveryPersonService(IDeliveryPerson repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DeliveryPerson>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<DeliveryPerson?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<DeliveryPerson> CreateAsync(CreateDeliveryPersonDto dto)
        {
            // Validation
            if (string.IsNullOrEmpty(dto.DeliveryName)) throw new AppException("Delivery name is required");
            if (string.IsNullOrEmpty(dto.MobileNo)) throw new AppException("Mobile number is required");
            if (string.IsNullOrEmpty(dto.LicenseNumber)) throw new AppException("License number is required");

            // DTO to Model mapping with proper defaults
            var deliveryPerson = new DeliveryPerson
            {
                DeliveryName = dto.DeliveryName,
                MobileNo = dto.MobileNo,
                Email = dto.Email,
                LicenseNumber = dto.LicenseNumber,
                otp = null, // No OTP during registration
                Status = DeliveryPersonStatus.Available, // Default status
                TotalDeliveries = 0, // Start with 0 deliveries
                AverageRating = 0.0 // Start with 0 rating
            };

            return await _repository.CreateAsync(deliveryPerson);
        }

        public async Task<DeliveryPerson> UpdateAsync(DeliveryPersonDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.DeliveryPersonId);
            if (existing == null)
                throw new KeyNotFoundException($"DeliveryPerson with ID {dto.DeliveryPersonId} not found");

            // Validation
            if (string.IsNullOrEmpty(dto.DeliveryName)) throw new AppException("Delivery name is required");
            if (string.IsNullOrEmpty(dto.MobileNo)) throw new AppException("Mobile number is required");
            if (string.IsNullOrEmpty(dto.Email)) throw new AppException("Email is required");

            // DTO to Model mapping
            existing.DeliveryName = dto.DeliveryName;
            existing.MobileNo = dto.MobileNo;
            existing.Email = dto.Email;
            existing.LicenseNumber = dto.LicenseNumber;
            existing.otp = dto.otp;
            existing.Status = Enum.Parse<DeliveryPersonStatus>(dto.Status);
            existing.TotalDeliveries = dto.TotalDeliveries;
            existing.AverageRating = dto.AverageRating;

            return await _repository.UpdateAsync(existing);
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new AppException($"DeliveryPerson with ID {id} not found");

            var parsed = Enum.Parse<DeliveryPersonStatus>(status);
            return await _repository.UpdateStatusAsync(id, parsed);
        }

        public async Task<int> GenerateOtpAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new AppException($"DeliveryPerson with ID {id} not found");

            // Generate 6-digit OTP
            var random = new Random();
            var otp = random.Next(100000, 999999);

            return await _repository.UpdateOtpAsync(id, otp);
        }

        public async Task CompleteDeliveryAsync(int deliveryPersonId)
        {
            await _repository.UpdateDeliveryStatsAsync(deliveryPersonId);
        }

        public async Task<(int totalDeliveries, double averageRating)> GetDeliveryStatsAsync(int deliveryPersonId)
        {
            return await _repository.CalculateStatsAsync(deliveryPersonId);
        }
    }
}
