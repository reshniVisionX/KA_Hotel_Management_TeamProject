using RestaurantBookingSystem.DTO;

namespace RestaurantBookingSystem.Interface.IService
{
    public interface IUserPreferenceService
    {
        Task<List<UserPreferenceDto>> GetAllPreferencesAsync();
        Task<UserPreferenceDto?> GetPreferenceAsync(int userId);
        Task<bool> CreatePreferenceAsync(CreateUserPreferenceDto createDto);
        Task<bool> UpdatePreferenceAsync(int userId, UpdateUserPreferenceDto updateDto);
        Task<bool> DeletePreferenceAsync(int userId);
    }
}
