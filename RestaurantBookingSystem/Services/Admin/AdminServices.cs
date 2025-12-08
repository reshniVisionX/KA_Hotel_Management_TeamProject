using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces.IRepository;
using RestaurantBookingSystem.Interfaces.IService;
using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Services
{
    public class AdminServices: IAdminService
    {
        private readonly IAdminRepository _adminRepo;

        public AdminServices(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public Task<IEnumerable<Restaurants>> GetAllRestaurantsAsync() =>
            _adminRepo.GetAllRestaurantsAsync();

        public Task<IEnumerable<Restaurants>> FilterRestaurants(
            int? id, string? city, RestaurantCategory? category,
            FoodType? type, string? managerName) =>
            _adminRepo.FilterRestaurants(id, city, category, type, managerName);

        public Task<Restaurants?> GetRestaurantByManagerIdAsync(int managerId) =>
            _adminRepo.GetRestaurantByManagerIdAsync(managerId);

        public Task<bool> ToggleRestaurantStatus(int restaurantId) =>
            _adminRepo.ToggleRestaurantStatus(restaurantId);

        public Task<IEnumerable<Users>> GetAllManagersAsync(int roleId) =>
            _adminRepo.GetAllManagersAsync(roleId);

        public Task<bool> ToggleManagerStatus(int managerId) =>
            _adminRepo.ToggleManagerStatus(managerId);

        public async Task<AnalyticsDTO> GetDashboardAnalyticsAsync()
        {
            var restaurants = await _adminRepo.GetAllRestaurantsAsync();
            var users = await _adminRepo.GetAllUsersAsync();
            var managers = await _adminRepo.GetAllManagerDetailsAsync();
            var reservations = await _adminRepo.GetAllReservationsAsync();
            var orders = await _adminRepo.GetAllOrdersAsync();
            

            var dto = new AnalyticsDTO
            {
                NoOfRestaurants = restaurants.Count(),
                NoOfUsers = users.Count(),
                NoOfManagers = managers.Count(),
                NoOfReservations = reservations.Count(),
                NoOfActiveUsers = users.Count(u => u.IsActive),
                NoOfActiveManagers = managers.Count(m => m.IsActive),
                NoOfActiveReservations = reservations.Count(r => r.Status == ReservationStatus.Reserved),
                DineInOrders = orders.Count(o => o.OrderType == OrderType.DineIn),
                DineOutOrders = orders.Count(o => o.OrderType == OrderType.DineOut),
                NoOfVegetarianHotels = restaurants.Count(r => r.RestaurantType == FoodType.Veg),
                NoOfNonVegetarianHotels = restaurants.Count(r => r.RestaurantType == FoodType.Nonveg)
            };

            return dto;
        }

        // ---------------- ENTIRE REVENUE ----------------
        public async Task<IEnumerable<EntireRevenueDTO>> GetEntireRevenueAnalyticsAsync(DateTime date)
        {
            var orders = await _adminRepo.GetAllOrdersAsync();
            var today = date.Date;
            var weekAgo = today.AddDays(-7);
            var monthAgo = today.AddMonths(-1);

            var dto = new EntireRevenueDTO
            {
                DailyRevenue = orders.Where(o => o.OrderDate.Date == today).Sum(o => o.TotalAmount),
                WeeklyRevenue = orders.Where(o => o.OrderDate >= weekAgo).Sum(o => o.TotalAmount),
                MonthlyRevenue = orders.Where(o => o.OrderDate >= monthAgo).Sum(o => o.TotalAmount),
                NoOfDailyOrders = orders.Count(o => o.OrderDate.Date == today),
                WeeklyOrders = orders.Count(o => o.OrderDate >= weekAgo),
                MonthlyOrders = orders.Count(o => o.OrderDate >= monthAgo)
            };

            return new List<EntireRevenueDTO> { dto };
        }

        // ---------------- RESTAURANT REVENUE ----------------
        public async Task<IEnumerable<RestaurantRevenueDTO>> GetRestaurantRevenueAsync(int restaurantId)
        {
            var orders = await _adminRepo.GetAllOrdersAsync();
            var restaurants = await _adminRepo.GetAllRestaurantsAsync();

            var today = DateTime.Today;
            var weekAgo = today.AddDays(-7);
            var monthAgo = today.AddMonths(-1);

            var restaurant = restaurants.FirstOrDefault(r => r.RestaurantId == restaurantId);

            if (restaurant == null)
                throw new AppException($"No restaurant found with ID {restaurantId}");

            var restOrders = orders.Where(o => o.RestaurantId == restaurantId).ToList();

            var dto = new RestaurantRevenueDTO
            {
                RestaurantId = restaurantId,
                RestaurantName = restaurant.RestaurantName,
                DailyRevenue = restOrders.Where(o => o.OrderDate.Date == today).Sum(o => o.TotalAmount),
                WeeklyRevenue = restOrders.Where(o => o.OrderDate >= weekAgo).Sum(o => o.TotalAmount),
                MonthlyRevenue = restOrders.Where(o => o.OrderDate >= monthAgo).Sum(o => o.TotalAmount),
                NoOfDailyOrders = restOrders.Count(o => o.OrderDate.Date == today),
                WeeklyOrders = restOrders.Count(o => o.OrderDate >= weekAgo),
                MonthlyOrders = restOrders.Count(o => o.OrderDate >= monthAgo)
            };

            return new List<RestaurantRevenueDTO> { dto };
        }
    }
}
