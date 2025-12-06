using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interfaces
{
    public interface ITableRepository
    {
        Task<List<DineIn>> GetRestaurantTablesAsync(int restaurantId);
        Task<List<DineIn>> GetAvailableTablesAsync(int restaurantId);
    }
}