using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Model.Manager;

namespace RestaurantBookingSystem.Interface
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurants>> GetAllAsync();
        Task<Restaurants?> GetByIdAsync(int id);
        Task<IEnumerable<Restaurants>> GetByManagerIdAsync(int managerId);
        Task<Restaurants> AddAsync(Restaurants restaurant);
        Task<Restaurants> UpdateAsync(Restaurants restaurant);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Restaurants>> SearchAsync(string? name, string? location);
        Task<int?> GetRestaurantIdByManagerIdAsync(int managerId);
        Task<ManagerDetails?> GetManagerByUserIdAsync(int userId);
    }
}