using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Manager;

namespace RestaurantBookingSystem.Interfaces.IService
{
    public interface IManagerRequestService
    {

        Task<bool> ProcessMonthlyPayoutToManagersAsync(PayoutDTO payout);
        Task<IEnumerable<PayoutDTO>> GetPayoutHistoryAsync(int managerId);

        Task<IEnumerable<ManagerRestaurantRequestDTO>> GetAllUnverifiedManagersAsync();

        Task<bool> VerifyManagerAsync(int managerId, bool isVerified);// if isverified is false, then reject

        Task<IEnumerable<ManagerDetails>> FilterManagersAsync(bool isActive, IsVerified? verification);



    }
}
