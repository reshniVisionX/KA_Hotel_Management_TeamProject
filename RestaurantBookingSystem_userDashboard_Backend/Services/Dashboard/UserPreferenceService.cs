using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Services
{
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly IUserPreferenceRepository _repository;

        public UserPreferenceService(IUserPreferenceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UserPreferenceDto>> GetAllPreferencesAsync()
        {
            var preferences = await _repository.GetAllUserPreferencesAsync();
            return preferences.Select(p => new UserPreferenceDto
            {
                PreferenceId = p.PreferenceId,
                UserId = p.UserId,
                Theme = p.Theme.ToString(),
                NotificationsEnabled = p.NotificationsEnabled,
                PreferredCity = p.PreferredCity,
                PreferredFoodType = p.PreferredFoodType
            }).ToList();
        }

        public async Task<UserPreferenceDto?> GetPreferenceAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            var preference = await _repository.GetUserPreferenceAsync(userId);
            if (preference == null) return null;

            return new UserPreferenceDto
            {
                PreferenceId = preference.PreferenceId,
                UserId = preference.UserId,
                Theme = preference.Theme.ToString(),
                NotificationsEnabled = preference.NotificationsEnabled,
                PreferredCity = preference.PreferredCity,
                PreferredFoodType = preference.PreferredFoodType
            };
        }

        public async Task<bool> CreatePreferenceAsync(CreateUserPreferenceDto createDto)
        {
            if (createDto.UserId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            var preference = new UserPreferences
            {
                UserId = createDto.UserId,
                Theme = !string.IsNullOrEmpty(createDto.Theme) ? Enum.Parse<UserTheme>(createDto.Theme, true) : UserTheme.Light,
                NotificationsEnabled = createDto.NotificationsEnabled,
                PreferredCity = createDto.PreferredCity,
                PreferredFoodType = createDto.PreferredFoodType
            };

            await _repository.CreateUserPreferenceAsync(preference);
            return true;
        }

        public async Task<bool> UpdatePreferenceAsync(int userId, UpdateUserPreferenceDto updateDto)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            var preference = await _repository.GetUserPreferenceAsync(userId);
            if (preference == null) return false;

            if (updateDto.Theme != null)
                preference.Theme = Enum.Parse<UserTheme>(updateDto.Theme, true);

            if (updateDto.NotificationsEnabled)
                preference.NotificationsEnabled = updateDto.NotificationsEnabled;

            if (!string.IsNullOrEmpty(updateDto.PreferredCity))
                preference.PreferredCity = updateDto.PreferredCity;

            if (!string.IsNullOrEmpty(updateDto.PreferredFoodType))
                preference.PreferredFoodType = updateDto.PreferredFoodType;

            return await _repository.UpdateUserPreferenceAsync(preference);
        }

        public async Task<bool> DeletePreferenceAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            return await _repository.DeleteUserPreferenceAsync(userId);
        }
    }
}