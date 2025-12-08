using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Model.Manager;
using RestaurantBookingSystem.Model.Restaurant;
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

        public async Task<(ManagerDetails Manager, Restaurants Restaurant)> RegisterManagerWithRestaurantAsync(ManagerRegisterDTO dto)
        {
            // 1. Check if user exists
            var user = await _userRepository.GetUserAsync(dto.UserId);
            if (user == null)
                throw new Exception("User does not exist");

            // 2. Check if user password matches manager password
            if (user.Password == dto.Password)
            {
                throw new Exception("Manager password cannot be same as user password.");
            }

            // 3. Check for duplicate manager email
            var existingManager = await _managerRepository.GetManagerByEmailAsync(dto.Email);
            if (existingManager != null)
                throw new Exception("Manager email already exists");

            // 4. Check if user is already a manager
            var existingUserManager = await _managerRepository.GetManagerByUserIdAsync(dto.UserId);
            if (existingUserManager != null)
                throw new Exception("User is already registered as a manager");

            // 5. Check for duplicate restaurant name in the same city
            var existingRestaurant = await _restaurantRepository.GetRestaurantByNameAndCityAsync(dto.Restaurant.RestaurantName, dto.Restaurant.City);
            if (existingRestaurant != null)
                throw new Exception("Restaurant with this name already exists in the city");

            // 6. Hash manager password
            string passwordHash = HashPassword(dto.Password);

            // 7. Create Manager Model
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

            // 8. Create Restaurant Model
            var restaurant = new Restaurants
            {
                RestaurantName = dto.Restaurant.RestaurantName,
                Description = dto.Restaurant.Description,
                Location = dto.Restaurant.Location,
                City = dto.Restaurant.City,
                ContactNo = dto.Restaurant.ContactNo,
                DeliveryCharge = dto.Restaurant.DeliveryCharge,
                RestaurantCategory = dto.Restaurant.RestaurantCategory,
                RestaurantType = dto.Restaurant.RestaurantType,
                ManagerId = createdManager.ManagerId,
                CreatedAt = DateTime.Now,
                IsActive = true
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
