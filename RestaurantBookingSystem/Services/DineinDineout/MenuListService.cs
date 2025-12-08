using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;
namespace RestaurantBookingSystem.Services
{
    public class MenuListService
    {
        private readonly IMenuList _repository;

        public MenuListService(IMenuList repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MenuList>> GetMenuByRestaurantIdAsync(int restaurantId)
        {
            var items = await _repository.GetMenuByRestaurantId(restaurantId);
            if (!items.Any())
            {
                // Check if restaurant exists
                var restaurantExists = await _repository.RestaurantExistsAsync(restaurantId);
                if (!restaurantExists)
                    throw new AppException($"Restaurant with ID {restaurantId} not found");
                throw new AppException($"No menu items found for restaurant {restaurantId}");
            }
            return items;
        }

        public async Task<MenuList> GetMenuItemByIdAsync(int id)
        {
            return await _repository.GetMenuItemById(id);
        }

        public async Task<IEnumerable<MenuList>> SearchMenuByNameAsync(string itemName)
        {
            var items = await _repository.SearchMenuByName(itemName);
            if (!items.Any())
                throw new AppException($"No menu items found with name '{itemName}'");
            return items;
        }
    }
}
