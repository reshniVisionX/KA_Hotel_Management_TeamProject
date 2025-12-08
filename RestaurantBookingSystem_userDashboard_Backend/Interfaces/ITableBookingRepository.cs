using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface.IRepository
{
    public interface ITableBookingRepository
    {
        Task<List<Reservation>> GetBookingsByUserIdAsync(int userId);
        Task<List<Reservation>> GetBookingsByRestaurantIdAsync(int restaurantId);
    }
}