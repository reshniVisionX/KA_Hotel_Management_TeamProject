using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Model.Manager;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interfaces.IRepository
{
    public interface IAdminRepository
    {
        // ------------------- Restaurants -------------------
        Task<IEnumerable<Restaurants>> GetAllRestaurantsAsync();
     
        Task<IEnumerable<Restaurants>> FilterRestaurants(
            int? id,
            string? city,
            RestaurantCategory? category,
            FoodType? type,
            string? managerName);

        Task<Restaurants?> GetRestaurantByManagerIdAsync(int managerId);
        Task<bool> ToggleRestaurantStatus(int restaurantId);

        // ------------------- Managers -------------------
        Task<IEnumerable<ManagerDetails>> GetAllManagersAsync();
        Task<bool> ToggleManagerStatus(int managerId);
        // ------------------- Analytics -------------------
        Task<IEnumerable<Restaurants>> GetAllRestaurantsForAnalyticsAsync();
        Task<IEnumerable<Orders>> GetAllOrdersAsync();
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<IEnumerable<ManagerDetails>> GetAllManagerDetailsAsync();
        Task<bool> UpdateVerificationAsync(int managerId, IsVerified status);
    }
}
