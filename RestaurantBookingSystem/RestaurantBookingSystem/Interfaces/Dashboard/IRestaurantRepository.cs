using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IRestaurantsRepository
    {
        Task<Restaurants> CreateRestaurantAsync(Restaurants restaurant);
        Task<Restaurants?> GetRestaurantByNameAndCityAsync(string name, string city);
    }
}
