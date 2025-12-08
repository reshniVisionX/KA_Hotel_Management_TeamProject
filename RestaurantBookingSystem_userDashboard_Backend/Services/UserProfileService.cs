using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userRepo;

        public UserProfileService(IUserProfileRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<UserProfileDto>> GetAllProfilesAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return users.Select(u => new UserProfileDto
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Mobile = u.Mobile,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                LastLogin = u.LastLogin
            }).ToList();
        }

        public async Task<UserProfileDto?> GetProfileAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            var user = await _userRepo.GetUserAsync(userId);
            if (user == null) return null;

            return new UserProfileDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Mobile = user.Mobile,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };
        }

        public async Task<bool> CreateProfileAsync(CreateUserProfileDto createDto)
        {
            if (string.IsNullOrWhiteSpace(createDto.FirstName))
                throw new ArgumentException("First name is required");

            if (string.IsNullOrWhiteSpace(createDto.LastName))
                throw new ArgumentException("Last name is required");

            if (string.IsNullOrWhiteSpace(createDto.Email))
                throw new ArgumentException("Email is required");

            if (string.IsNullOrWhiteSpace(createDto.Password))
                throw new ArgumentException("Password is required");

            // Check for duplicate email
            var existingUser = await _userRepo.GetUserByEmailAsync(createDto.Email);
            if (existingUser != null)
                throw new ArgumentException("Email already exists");

            // Password strength validation
            if (createDto.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long");

            var user = new Users
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
                Password = createDto.Password,
                Mobile = createDto.Mobile,
                IsActive = true,
                CreatedAt = DateTime.Now,
                LastLogin = DateTime.Now
            };

            await _userRepo.CreateUserAsync(user);
            return true;
        }

        public async Task<bool> UpdateProfileAsync(int userId, UpdateUserProfileDto updateDto)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            var user = await _userRepo.GetUserAsync(userId);
            if (user == null) return false;

            // Check for duplicate email if email is being updated
            if (!string.IsNullOrWhiteSpace(updateDto.Email) && updateDto.Email != user.Email)
            {
                var existingUser = await _userRepo.GetUserByEmailAsync(updateDto.Email);
                if (existingUser != null)
                    throw new ArgumentException("Email already exists");
            }

            user.FirstName = updateDto.FirstName ?? user.FirstName;
            user.LastName = updateDto.LastName ?? user.LastName;
            user.Email = updateDto.Email ?? user.Email;
            user.Mobile = updateDto.Mobile ?? user.Mobile;

            return await _userRepo.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteProfileAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            return await _userRepo.DeleteUserAsync(userId);
        }
    }
}