using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Manager;

namespace RestaurantBookingSystem.Interfaces.IRepository
{
    public interface IManagerRequestRepository
    {
        Task<bool> ProcessMonthlyPayoutToManagersAsync(ManagerPayment payout);
        Task<IEnumerable<ManagerPayment>> GetPayoutHistoryAsync(int managerId);

        Task<IEnumerable<ManagerDetails>> GetAllUnverifiedManagersAsync();

        Task<bool> VerifyManagerAsync(int managerId, bool isVerified);// if isverified is false, then reject

        Task<IEnumerable<ManagerDetails>> FilterManagersAsync(bool isActive, IsVerified? verification);

    }
}
