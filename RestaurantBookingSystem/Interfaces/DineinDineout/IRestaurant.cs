using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IRestaurant
    {
        Task<IEnumerable<Restaurants>> GetAllRestaurants();
        Task<Restaurants> GetRestaurantById(int id);
        Task<IEnumerable<MenuList>> GetMenuByRestaurantId(int restaurantId);
        Task<IEnumerable<Restaurants>> SearchRestaurantsAsync(string? name, string? city);
        Task<bool> RestaurantExistsAsync(int restaurantId);
    }
}
