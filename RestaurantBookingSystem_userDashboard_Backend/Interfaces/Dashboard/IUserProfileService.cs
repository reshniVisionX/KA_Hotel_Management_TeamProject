using RestaurantBookingSystem.DTO;

namespace RestaurantBookingSystem.Interface.IService
{
    public interface IUserProfileService
    {
        Task<List<UserProfileDto>> GetAllProfilesAsync();
        Task<UserProfileDto?> GetProfileAsync(int userId);
        Task<bool> CreateProfileAsync(CreateUserProfileDto createDto);
        Task<bool> UpdateProfileAsync(int userId, UpdateUserProfileDto updateDto);
        Task<bool> DeleteProfileAsync(int userId);
    }
}
