using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IMenuList
    {
        Task<IEnumerable<MenuList>> GetMenuByRestaurantId(int restaurantId);
        Task<MenuList> GetMenuItemById(int id);
        Task<IEnumerable<MenuList>> SearchMenuByName(string itemName);
        Task<bool> RestaurantExistsAsync(int restaurantId);
    }
}
