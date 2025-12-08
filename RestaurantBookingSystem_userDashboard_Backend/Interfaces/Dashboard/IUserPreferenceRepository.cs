using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Interface.IRepository
{
    public interface IUserPreferenceRepository
    {
        Task<List<UserPreferences>> GetAllUserPreferencesAsync();
        Task<UserPreferences?> GetUserPreferenceAsync(int userId);
        Task<UserPreferences> CreateUserPreferenceAsync(UserPreferences preference);
        Task<bool> UpdateUserPreferenceAsync(UserPreferences preference);
        Task<bool> DeleteUserPreferenceAsync(int userId);
    }
}
