using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Model.Manager;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantBookingSystem.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IRestaurantsRepository _restaurantRepository;
        private readonly IUserProfileRepository _userRepository;

        public ManagerService(IManagerRepository managerRepository, IRestaurantsRepository restaurantRepository, IUserProfileRepository userRepository)
        {
            _managerRepository = managerRepository;
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
        }

        public async Task<(ManagerDetails Manager, Restaurants Restaurant)> RegisterManagerWithRestaurantFormAsync(ManagerRegisterDTO dto)
        {
            // 1. Verify user
            var user = await _userRepository.GetUserAsync(dto.UserId);
            if (user == null)
                throw new AppException("User does not exist");

            // 2. Password check
            if (user.Password == dto.Password)
                throw new AppException("Manager password cannot be same as user password");

            // 3. Email duplicate check
            var existingManager = await _managerRepository.GetManagerByEmailAsync(dto.Email);
            if (existingManager != null)
                throw new AppException("Manager email already exists");

            // 4. User already manager?
            var existingUserManager = await _managerRepository.GetManagerByUserIdAsync(dto.UserId);
            if (existingUserManager != null)
                throw new AppException("User is already registered as a manager");

            // 5. Restaurant duplicate (city + name)
            var existingRestaurant = await _restaurantRepository
                .GetRestaurantByNameAndCityAsync(dto.RestaurantName, dto.City);

            if (existingRestaurant != null)
                throw new AppException("Restaurant with this name already exists in the city");

            // 6. Hash password
            string passwordHash = HashPassword(dto.Password);

            // 7. Create Manager
            var manager = new ManagerDetails
            {
                ManagerName = dto.ManagerName,
                UserId = dto.UserId,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true
            };

            var createdManager = await _managerRepository.CreateManagerAsync(manager);

            // 8. Convert file to byte[]
            byte[]? imageBytes = null;

            if (dto.Image != null)
            {
                using var ms = new MemoryStream();
                await dto.Image.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            // 9. Create Restaurant
            var restaurant = new Restaurants
            {
                RestaurantName = dto.RestaurantName,
                Description = dto.Description,
                Ratings = dto.Ratings,
                RestaurantCategory = dto.RestaurantCategory,
                RestaurantType = dto.RestaurantType,
                Location = dto.Location,
                City = dto.City,
                ContactNo = dto.ContactNo,
                DeliveryCharge = dto.DeliveryCharge,
                ManagerId = createdManager.ManagerId,
                Images = imageBytes,
                IsActive = false,
                CreatedAt = DateTime.Now
            };

            var createdRestaurant = await _restaurantRepository.CreateRestaurantAsync(restaurant);

            return (createdManager, createdRestaurant);
        }


        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}