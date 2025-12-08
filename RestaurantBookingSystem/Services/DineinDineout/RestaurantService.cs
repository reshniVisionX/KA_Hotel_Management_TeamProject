using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Services
{
    public class RestaurantService
    {
        private readonly IRestaurant _repository;

        public RestaurantService(IRestaurant repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Restaurants>> GetAllRestaurantsAsync()
        {
            return await _repository.GetAllRestaurants();
        }

        public async Task<Restaurants> GetRestaurantByIdAsync(int id)
        {
            return await _repository.GetRestaurantById(id);
        }

        public async Task<IEnumerable<MenuList>> GetMenuByRestaurantIdAsync(int restaurantId)
        {
            var restaurantExists = await _repository.RestaurantExistsAsync(restaurantId);
            if (!restaurantExists)
                throw new AppException($"Restaurant with ID {restaurantId} not found");

            var menu = await _repository.GetMenuByRestaurantId(restaurantId);
            if (!menu.Any())
                throw new AppException($"No menu items found for restaurant {restaurantId}");

            return menu;
        }

        public async Task<IEnumerable<Restaurants>> SearchRestaurantsAsync(string? name, string? city)
        {
            var restaurants = await _repository.SearchRestaurantsAsync(name, city);
            if (!restaurants.Any())
            {
                var searchCriteria = new List<string>();
                if (!string.IsNullOrEmpty(name)) searchCriteria.Add($"name '{name}'");
                if (!string.IsNullOrEmpty(city)) searchCriteria.Add($"city '{city}'");
                
                var criteria = searchCriteria.Any() ? string.Join(" and ", searchCriteria) : "the given criteria";
                throw new AppException($"No restaurants found for {criteria}");
            }
            return restaurants;
        }
    }
}
