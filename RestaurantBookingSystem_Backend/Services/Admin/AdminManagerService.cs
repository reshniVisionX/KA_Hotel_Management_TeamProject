using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces.IRepository;
using RestaurantBookingSystem.Interfaces.IService;
using RestaurantBookingSystem.Model.Manager;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Services
{
    public class AdminManagerService:IManagerRequestService
    {
        private readonly IManagerRequestRepository _managerRepo;

        public AdminManagerService(IManagerRequestRepository managerRepo)
        {
            _managerRepo = managerRepo;
        }

        // ------------------- PAYOUTS -------------------
        public async Task<bool> ProcessMonthlyPayoutToManagersAsync(PayoutDTO payout)
        {
            // Validation rules
            if (payout == null)
                throw new AppException( "Payout details are required.");

            if (payout.ManagerId <= 0)
                throw new AppException("Invalid Manager ID.");

            if (payout.RestaurantId <= 0)
                throw new AppException("Invalid Restaurant ID.");

            if (payout.Amount <= 0)
                throw new AppException("Payout amount must be greater than zero.");

            var payment = new ManagerPayment
            {
                ManagerId = payout.ManagerId,
                RestaurantId = payout.RestaurantId,
                Amount = payout.Amount,
                Remarks = payout.Remarks,
                PaymentStatus = payout.PaymentStatus,
                PaymentDate = payout.PaymentDate,
                CreatedAt = DateTime.Now
            };
            // Proceed with repo call
            return await _managerRepo.ProcessMonthlyPayoutToManagersAsync(payment);
        }

        public async Task<IEnumerable<PayoutDTO>> GetPayoutHistoryAsync(int managerId)
        {
            if (managerId <= 0)
                throw new AppException("Invalid Manager ID.");


            var payments = await _managerRepo.GetPayoutHistoryAsync(managerId);
            var payoutDtos = payments.Select(p => new PayoutDTO
            {
                ManagerId = p.ManagerId,
                RestaurantId = p.RestaurantId,
                Amount = p.Amount,
                Remarks = p.Remarks ?? "No remarks provided.",
                PaymentStatus = p.PaymentStatus ?? "Unknown",
                PaymentDate = p.PaymentDate
            }).ToList();

            return payoutDtos;


        }

        // ------------------- MANAGER VERIFICATION -------------------
        public async Task<IEnumerable<ManagerRestaurantRequestDTO>> GetAllUnverifiedManagersAsync()
        {
            // Step 1: Fetch from repository
            var managers = await _managerRepo.GetAllUnverifiedManagersAsync();

            // Defensive: Handle null or empty
            if (managers == null || !managers.Any())
                return Enumerable.Empty<ManagerRestaurantRequestDTO>();

          
            var result = managers.Select(m => new ManagerRestaurantRequestDTO
            {
                ManagerId = m.ManagerId,
                ManagerName = m.ManagerName ?? string.Empty,
                Email = m.Email ?? string.Empty,
                PhoneNumber = m.PhoneNumber ?? string.Empty,
                Verification = m.Verification ?? IsVerified.Unverified,
                Restaurants = m.Restaurants ?? new List<Restaurants>()
            }).ToList();

            return result;
        }


        public async Task<bool> VerifyManagerAsync(int managerId, bool isVerified)
        {
            if (managerId <= 0)
                throw new AppException("Invalid Manager ID.");

            return await _managerRepo.VerifyManagerAsync(managerId, isVerified);
        }

        public async Task<IEnumerable<ManagerDetails>> FilterManagersAsync(bool isActive, IsVerified? verification)
        {
            return await _managerRepo.FilterManagersAsync(isActive, verification);
        }
    }
}
